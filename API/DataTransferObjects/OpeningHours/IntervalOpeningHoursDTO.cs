﻿using System.Collections.Generic;

namespace DataTransferObjects.OpeningHours
{
    public class IntervalOpeningHoursDTO
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int IntervalMinutes { get; set; }
        public SortedDictionary<string, ICollection<IntervalOpeningHourDTO>> Shifts { get; set; }
    }
}
