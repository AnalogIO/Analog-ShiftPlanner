﻿using System.Collections.Generic;

namespace Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual EmployeeTitle EmployeeTitle { get; set; }
        public bool Active { get; set; }
        public int WantShifts { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; }
        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}