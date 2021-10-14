using System;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services
{
    public interface IEmployeeStore
    {
        Task<Employee> FindEmployee(Guid id);
        Task CreateEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Guid id);
    }
}
