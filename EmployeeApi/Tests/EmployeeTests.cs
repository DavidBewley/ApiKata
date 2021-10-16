using System;
using System.Linq;
using Bogus;
using Core.Models;
using Core.Processors;
using Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests
{
    public class EmployeeTests
    {
        private static Employee _processedEmployee;
        private static Employee _expectedEmployee;

        private static Mock<IEmployeeStore> _employeeStore;

        private static readonly Faker<Employee> EmployeeFaker = new Faker<Employee>()
            .RuleFor(e => e.Id, Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.StartDate, f => f.Date.Past());

        public class WhenAnEmployeeIsSearchedForAndIsPresentInTheDataStore
        {
            public WhenAnEmployeeIsSearchedForAndIsPresentInTheDataStore()
            {
                _expectedEmployee = EmployeeFaker.Generate(1).Single();

                _employeeStore = new Mock<IEmployeeStore>();
                _employeeStore
                    .Setup(s => s.FindEmployee(It.IsAny<Guid>()))
                    .ReturnsAsync(_expectedEmployee);

                var processor = new EmployeeProcessor(_employeeStore.Object);
                _processedEmployee = processor.FindEmployee(_expectedEmployee.Id).Result;
            }

            [Fact]
            public void EmployeeIsFound()
                => _processedEmployee.Should().NotBeNull();

            [Fact]
            public void ExpectedEmployeeIsFound()
                => _processedEmployee.Should().Be(_expectedEmployee);

            [Fact]
            public void DataStoreIsCalledWithCorrectId()
                => _employeeStore.Verify(e => e.FindEmployee(It.Is<Guid>(i => i == _expectedEmployee.Id)));
        }

        public class WhenAnEmployeeIsSearchedForAndIsNotPresentInTheDataStore
        {
            private readonly Guid _searchId;

            public WhenAnEmployeeIsSearchedForAndIsNotPresentInTheDataStore()
            {
                _employeeStore = new Mock<IEmployeeStore>();

                _searchId = Guid.NewGuid();
                var processor = new EmployeeProcessor(_employeeStore.Object);
                _processedEmployee = processor.FindEmployee(_searchId).Result;
            }

            [Fact]
            public void EmployeeIsNotFound()
                => _processedEmployee.Should().BeNull();

            [Fact]
            public void DataStoreIsCalledWithCorrectId()
                => _employeeStore.Verify(e => e.FindEmployee(It.Is<Guid>(i => i == _searchId)));
        }

        public class WhenCreatingAnEmployee
        {
            private readonly string _createEmployeeGuid;
            private Employee _callBackEmployee;

            public WhenCreatingAnEmployee()
            {
                var createEmployee = EmployeeFaker.Generate(1).Single();
                _createEmployeeGuid = createEmployee.Id.ToString();

                _employeeStore = new Mock<IEmployeeStore>();
                _employeeStore
                    .Setup(e => e.CreateEmployee(It.IsAny<Employee>()))
                    .Callback<Employee>(e => _callBackEmployee = e);

                var processor = new EmployeeProcessor(_employeeStore.Object);
                _processedEmployee = processor.CreateEmployee(createEmployee).Result;
            }

            [Fact]
            public void EmployeeIsGivenANewGuid()
                => _processedEmployee.Id.ToString().Should().NotBe(_createEmployeeGuid);

            [Fact]
            public void DataStoreCreateIsCalledWithGivenEmployee()
                => _employeeStore.Verify(e => e.CreateEmployee(It.Is<Employee>(e=>e == _callBackEmployee)));
        }

    }
}
