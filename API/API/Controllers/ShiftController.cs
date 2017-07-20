﻿using System;
using System.Web.Http;
using API.Authorization;
using API.Logic;
using DataTransferObjects.Shift;
using System.Net.Http;
using System.Net;
using System.Runtime.Remoting;
using Data.Services;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage shifts
    /// </summary>
    [Authorize(Roles = "Manager")]
    [RoutePrefix("api/shifts")]
    public class ShiftController : ApiController
    {
        private readonly IShiftService _shiftService;
        private readonly IAuthManager _authManager;

        /// <summary>
        /// The constructor of the shift controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="shiftService"></param>
        public ShiftController(IAuthManager authManager, IShiftService shiftService)
        {
            _authManager = authManager;
            _shiftService = shiftService;
        }

        /// <summary>
        /// Returns all shifts of the specified organization in the 'Authorization' header
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("No manager found with the given name");
            return Ok(Mapper.Map(_shiftService.GetByOrganization(employee.Organization.Id)));
        }

        /// <summary>
        /// Returns the shift for the given id in the parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.GetShift(id, organization.Id);
            if(shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes the shift with the given id in the parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("No manager found with the given token");

            _shiftService.DeleteShift(id, employee.Organization.Id);
            
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        /// <summary>
        /// Updates the shift with the id in the parameter with the content in the body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        public IHttpActionResult Update(int id, UpdateShiftDTO shiftDto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("No manager found with the given token");

            var shift = _shiftService.UpdateShift(id, employee.Organization.Id, shiftDto);

            if(shift != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("The shift could not be updated!");
        }

        /// <summary>
        /// Creates a shift with the given employees for the given time defined in the body
        /// </summary>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        public IHttpActionResult Create(CreateShiftDTO shiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("No manager found with the given token");

            var shift = _shiftService.CreateShift(employee.Organization, shiftDto);
            if (shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return BadRequest("Could not create shift!");
        }

        /// <summary>
        /// Gets the shifts for today
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpGet, Route("today")]
        public IHttpActionResult Today()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var now = DateTime.Now.Date;
            var end = now.AddDays(1).AddTicks(-1);

            var shifts = Mapper.Map(_shiftService.GetByOrganization(organization.Id,now,end));

            return Ok(shifts);
        }


        /// <summary>
        /// Gets the shifts currently ongoing with the corresponding employees planned to be on the shift and the employees checked-in on that shift
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpGet, Route("ongoing")]
        public IHttpActionResult OnGoing()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shifts = Mapper.Map(_shiftService.GetOngoingShiftsByOrganization(organization.Id));

            return Ok(new { Shifts = shifts });
        }

        /// <summary>
        /// Checks in the employee with the given employee id in the parameters for the given shift id in the parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpPost, Route("{id}/checkin")]
        public IHttpActionResult CheckIn(int id, int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var checkIn = _shiftService.CheckInEmployee(id, employeeId, organization.Id);
            if (checkIn != null)
            {
                return Ok(Mapper.Map(checkIn));
            }
            return BadRequest("Could not check-in the employee - try again!");
        }

        /// <summary>
        /// Adds the employees from the body to the given shift
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpPost, Route("{id}/addEmployees")]
        public IHttpActionResult AddEmployees(int id, AddEmployeesDTO employees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.AddEmployeesToShift(id, organization.Id, employees);
            if (shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return BadRequest("Could not add the employees");
        }

        /// <summary>
        /// Creates a shift with the given employees from now (rounded up to nearest 15 minutes) and for the next xx minutes defined in the body
        /// </summary>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpPost, Route("createoutsideschedule")]
        public IHttpActionResult CreateOutsideSchedule(CreateShiftDTO shiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.CreateLimitedShift(organization, shiftDto, 300); // Create shift if it doesnt exceed a duration of 5 hours
            if (shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return BadRequest("Could not create shift!");
        }
    }
}