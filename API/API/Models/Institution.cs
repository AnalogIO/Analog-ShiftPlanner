﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeeTitle> EmployeeTitles { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}