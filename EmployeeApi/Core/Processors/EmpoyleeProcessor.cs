using System;
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

        public async Task<Employee> FindEmployee(Guid id)
            => await _employeeStore.FindEmployee(id);

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            await _employeeStore.CreateEmployee(employee);
            return employee;
        }
    }
}
