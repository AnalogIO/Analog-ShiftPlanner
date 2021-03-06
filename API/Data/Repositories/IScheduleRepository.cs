﻿using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IScheduleRepository
    {
        Schedule Create(Schedule schedule);
        IEnumerable<Schedule> ReadFromOrganization(int organizationId);
        Schedule Read(int id, int organizationId);
        int Update(Schedule schedule);
        void Delete(int id, int organizationId);
        void DeleteScheduledShift(int scheduleId, int scheduledShiftId, int organizationId);
        IEnumerable<ScheduledShift> GetScheduledShifts(IEnumerable<int> scheduledShiftIds);
        void DeletePreferences(IEnumerable<Preference> prefences);
        void DeletePreference(Preference prefence);
    }
}
