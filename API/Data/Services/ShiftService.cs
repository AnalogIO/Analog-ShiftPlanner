﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Shift;

namespace Data.Services
{
    /// <summary>
    /// Implementation of IShiftService that uses Repositories for data access.
    /// </summary>
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Injection constructor.
        /// </summary>
        /// <param name="shiftRepository">An IShiftRepository implementation.</param>
        /// <param name="institutionRepository">An IInstitution implementation.</param>
        public ShiftService(IShiftRepository shiftRepository, IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository)
        {
            _shiftRepository = shiftRepository;
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey)
        {
            if (_organizationRepository.Exists(shortKey))
                return _shiftRepository.ReadFromOrganization(shortKey);
            return null;
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int)"/>
        public IEnumerable<Shift> GetByOrganization(int id)
        {
            if (_organizationRepository.Exists(id))
                return _shiftRepository.ReadFromOrganization(id);
            return null;
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey, DateTime from, DateTime to)
        {
            return GetByOrganization(shortKey).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(int id, DateTime from, DateTime to)
        {
            return GetByOrganization(id).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey, DateTime date)
        {
            return GetByOrganization(shortKey, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(int id, DateTime date)
        {
            return GetByOrganization(id, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByOrganization(string)"/>
        public IEnumerable<Shift> GetOngoingShiftsByOrganization(string shortKey)
        {
            var now = DateTime.Now;
            return GetByOrganization(shortKey).Where(shift => shift.Start <= now && now <= shift.End);
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByOrganization(int)"/>
        public IEnumerable<Shift> GetOngoingShiftsByOrganization(int id)
        {
            var now = DateTime.Now;
            return GetByOrganization(id).Where(shift => shift.Start <= now && now <= shift.End);
        }

        public CheckIn CheckInEmployee(int shiftId, int employeeId, int institutionId)
        {
            var shift = _shiftRepository.Read(shiftId, institutionId);
            if (shift == null) return null;
            if (shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employeeId) != null) return null;
            var employee = _employeeRepository.Read(employeeId, institutionId);
            if (employee == null) return null;
            var now = DateTime.Now;
            var checkIn = new CheckIn { Employee = employee, Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) };
            shift.CheckIns.Add(checkIn);
            return _shiftRepository.Update(shift) > 0 ? shift.CheckIns.LastOrDefault() : null;
        }

        public Shift CreateShiftOutsideSchedule(CreateShiftOutsideScheduleDTO shiftDto, Organization organization)
        {
            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();
            if (employees == null) return null;
            var now = DateTime.Now;
            var start = Toolbox.RoundUp(now, TimeSpan.FromMinutes(15));
            var end = start.AddMinutes(shiftDto.OpenMinutes);

            var shift = new Shift { Start = start, End = end, CheckIns = new List<CheckIn>(), Employees = employees, Organization = organization };
            return _shiftRepository.Create(shift);
        }

        public Shift GetShift(int shiftId, int organizationId)
        {
            return _shiftRepository.Read(shiftId, organizationId);
        }

        public void DeleteShift(int shiftId, int organizationId)
        {
            _shiftRepository.Delete(shiftId, organizationId);
        }

        public Shift UpdateShift(int shiftId, int organizationId, UpdateShiftDTO updateShiftDto)
        {
            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) return null;

            var employees = _employeeRepository.ReadFromOrganization(organizationId).Where(x => updateShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            var start = DateTimeOffset.Parse(updateShiftDto.Start).UtcDateTime;
            var end = DateTimeOffset.Parse(updateShiftDto.End).UtcDateTime;

            shift.Employees = employees;
            shift.CheckIns = shift.CheckIns.Where(x => updateShiftDto.CheckInIds.Contains(x.Id)).ToList();
            shift.Start = start;
            shift.End = end;

            return _shiftRepository.Update(shift) > 0 ? shift : null;
        }
    }
}