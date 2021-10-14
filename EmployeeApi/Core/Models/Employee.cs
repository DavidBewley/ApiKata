using System;

namespace Core.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public Employee() { }

        public Employee(Guid id, string name, DateTime startDate)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
        }
    }
}
