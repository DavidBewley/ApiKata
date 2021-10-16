using System;
using System.Collections.Generic;
using Core.Models;

namespace Core.Services
{
    public interface IEmployeeStore
    {
        List<Employee> FindEmployees();
        Employee FindEmployee(Guid id);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Guid id);
    }
}
