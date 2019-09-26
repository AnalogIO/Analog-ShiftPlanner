﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Data.Models
{
    public class EmployeeAssignment
    {
        [Key]
        public int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ScheduledShift ScheduledShift { get; set; }
        public bool IsLocked { get; set; }
    }
}
