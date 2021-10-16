using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;

namespace Core.Processors
{
    public class EmployeeProcessor
    {
        private readonly IEmployeeStore _employeeStore;

        public EmployeeProcessor(IEmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
        }

        public List<Employee> FindEmployees()
            => _employeeStore.FindEmployees();

        public Employee FindEmployee(Guid id)
            => _employeeStore.FindEmployee(id);

        public Employee CreateEmployee(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            _employeeStore.CreateEmployee(employee);
            return employee;
        }

        public Employee UpdateEmployee(Employee updateEmployee)
        {
            var storedEmployee = _employeeStore.FindEmployee(updateEmployee.Id);
            if (storedEmployee == null)
                return null;

            _employeeStore.UpdateEmployee(updateEmployee);
            return updateEmployee;
        }

        public Employee DeleteEmployee(Guid id)
        {
            var storedEmployee = _employeeStore.FindEmployee(id);
            if (storedEmployee == null)
                return null;

            _employeeStore.DeleteEmployee(id);
            return storedEmployee;
        }
    }
}
