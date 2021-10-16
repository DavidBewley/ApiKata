using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Models;

namespace Core.Services.Concrete
{
    public class EmployeeStoreApi : IEmployeeStore
    {
        private const string FilePath = "Employees.txt";

        public EmployeeStoreApi()
        {
            var fileExists = File.Exists(FilePath);

            if (fileExists)
                return;

            using (File.Create(FilePath));
        }

        public List<Employee> FindEmployees()
        {
            var lines = File.ReadAllLines(FilePath);
            lines = lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();

            return lines
                .Select(line => line.Split(','))
                .Select(lineSplit => new Employee(Guid.Parse(lineSplit[0]), lineSplit[1], DateTime.Parse(lineSplit[2])))
                .ToList();
        }

        public Employee FindEmployee(Guid id)
        {
            var allEmployees = FindEmployees();
            return allEmployees.FirstOrDefault(e => e.Id == id);
        }

        public void CreateEmployee(Employee employee)
        {
            using (var sw = File.AppendText(FilePath))
                WriteEmployee(sw, employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            var allEmployees = FindEmployees();
            allEmployees = allEmployees.Where(e => e.Id != employee.Id).ToList();
            allEmployees.Add(employee);
            WriteAllEmployees(allEmployees);
        }

        public void DeleteEmployee(Guid id)
        {
            var allEmployees = FindEmployees();
            allEmployees = allEmployees.Where(e => e.Id != id).ToList();
            WriteAllEmployees(allEmployees);
        }

        private void WriteAllEmployees(List<Employee> employees)
        {
            using (var sw = File.CreateText(FilePath))
                foreach (var employee in employees)
                    WriteEmployee(sw, employee);
        }

        private void WriteEmployee(StreamWriter sw, Employee employee)
            => sw.WriteLine($"{employee.Id},{employee.Name},{employee.StartDate}");
    }
}
