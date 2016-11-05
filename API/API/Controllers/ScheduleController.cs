﻿using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
    [RoutePrefix("api/schedules")]
    public class ScheduleController : ApiController
    {
        private readonly AuthManager _authManager;
        private readonly ScheduleService _scheduleService;

        public ScheduleController()
        {
            _authManager = UnityConfig.GetConfiguredContainer().Resolve<AuthManager>();
            _scheduleService = UnityConfig.GetConfiguredContainer().Resolve<ScheduleService>();
        }

        // GET api/schedules
        /// <summary>
        /// Gets all the schedules.
        /// </summary>
        /// <returns>
        /// Returns an array of schedules.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedules = _scheduleService.GetSchedules(manager);
            return Ok(schedules); // proper dto should be used here
        }

        // GET api/schedules/{id}
        /// <summary>
        /// Gets the schedule with the given id.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>
        /// Returns the schedule with the given id. 
        /// If no schedule is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, manager);
            if (schedule != null)
            {
                return Ok(schedule); // proper dto should be used here
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/schedules
        /// <summary>
        /// Creates the schedule from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the schedule gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.CreateSchedule(scheduleDto, manager);
            if (schedule != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("The schedule could not be created!");
        }

        // DELETE /api/schedules/{id}
        /// <summary>
        /// Deletes the schedule with the specified id.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>Returns 'No Content' (204) if the schedule gets deleted.</returns>
        [HttpDelete, AdminFilter, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            _scheduleService.DeleteSchedule(id, manager);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        // POST api/schedules/{id}
        /// <summary>
        /// Creates the scheduled shift to the schedule with the given id from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shift gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("{id}")]
        public IHttpActionResult CreateScheduledShift(int id, CreateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.CreateScheduledShift(scheduledShiftDto, manager.Institution, id);

            if (scheduledShift != null) {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }

            return BadRequest("The schedule could not be created!");
        }

        // PUT api/schedules/{id}
        /// <summary>
        /// Updates the scheduled shift with the given id from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'No Content' (204) if the scheduled shift gets updated.
        /// </returns>
        [HttpPut, AdminFilter, Route("{id}")]
        public IHttpActionResult UpdateScheduledShift(int id, UpdateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.UpdateScheduledShift(scheduledShiftDto, manager.Institution, id);

            if (scheduledShift != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("The schedule could not be updated!");
        }

        // POST api/schedules/{id}/createmultiple
        /// <summary>
        /// Creates the scheduled shifts to the schedule with the given id from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shifts gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("{id}/createmultiple")]
        public IHttpActionResult CreateMultipleScheduledShift(int id, IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShifts = _scheduleService.CreateScheduledShifts(scheduledShiftsDto, manager.Institution, id);

            if (scheduledShifts != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }

            return BadRequest("The schedule could not be created!");
        }

    }
}