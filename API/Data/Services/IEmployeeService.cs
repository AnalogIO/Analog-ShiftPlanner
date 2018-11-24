﻿using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Employee;
using PodioAPI.Models;
using PodioAPI.Utils;

namespace Data.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Employee employee);
        Employee CreateEmployeeFromPodio(CreateEmployeeDTO employeeDto, Models.Organization organization);
        IEnumerable<Employee> CreateManyEmployees(CreateEmployeeDTO[] employeeDtos, Employee employee);
        void DeleteEmployee(int employeeId, Employee employee);
        Employee GetEmployee(int id, int organizationId);
        Employee GetEmployee(int id, string shortKey);
        IEnumerable<Employee> GetEmployees(int organizationId);
        IEnumerable<Employee> GetEmployees(string shortKey);
        IEnumerable<Employee> GetEmployeesByActivity(int organizationId, bool active);
        IEnumerable<Employee> GetEmployeesByActivity(string shortKey, bool active);

        Employee UpdateEmployee(UpdateEmployeeDTO employeeDto, Employee employee, Photo photo);
        Employee Login(string email, string password);
        Friendship CreateFriendship(Employee employee, int friendId);
        void DeleteFriendship(Employee employee, int friendId);
        void ResetPassword(int id, int organizationId);
        int SyncEmployees(string shortKey);
    }
}