﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;
using Data.Exceptions;
using System.Data;

namespace Data.Npgsql.Repositories
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;

        public EmployeeRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Employee Create(Employee employee)
        {
            if (_context.Employees.Any(x => x.Email == employee.Email)) throw new ForbiddenException("An employee already exist with the given email");

            _context.Employees.Add(employee);
            return _context.SaveChanges() > 0 ? employee : null;
        }

        public IEnumerable<Employee> CreateMany(IEnumerable<Employee> employees)
        {
            _context.Employees.AddRange(employees);
            return _context.SaveChanges() > 0 ? employees : null;
        }

        public void Delete(int id, int organizationId)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
            if (employee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
 
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public IEnumerable<Employee> ReadFromOrganization(int organizationId)
        {
            return _context.Employees
                .Include(x => x.EmployeeTitle)
                .Include(x => x.Photo)
                .Where(e => e.Organization.Id == organizationId).OrderBy(x => x.Id);
        }

        public IEnumerable<Employee> ReadFromOrganization(string shortKey)
        {
            return _context.Employees
                .Include(emp => emp.EmployeeTitle)
                .Include(emp => emp.Photo)
                .Where(e => e.Organization.ShortKey == shortKey);
        }

        public Employee Read(int id, int organizationId)
        {
            return _context.Employees
                .Include(x => x.EmployeeTitle)
                .FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
        }

        public Employee Read(int id, string shortKey)
        {
            return _context.Employees
                .Include(employee => employee.EmployeeTitle)
                .Include(employee => employee.Photo)
                .FirstOrDefault(x => x.Id == id && x.Organization.ShortKey == shortKey);
        }

        public int Update(Employee employee)
        {
            var dbEmployee = _context.Employees.Single(e => e.Id == employee.Id);

            dbEmployee.Email = employee.Email;
            dbEmployee.FirstName = employee.FirstName;
            dbEmployee.LastName = employee.LastName;
            dbEmployee.EmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == employee.EmployeeTitle.Id);

            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}