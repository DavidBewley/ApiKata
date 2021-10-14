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
        private static Employee _foundEmployee;
        private static Employee _expectedEmployee;

        private static readonly Mock<IEmployeeStore> EmployeeStore = new Mock<IEmployeeStore>();

        private static readonly Faker<Employee> EmployeeFaker = new Faker<Employee>()
            .RuleFor(e => e.Id, Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.StartDate, f => f.Date.Past());


        public class WhenAnEmployeeIsSearchedForAndIsPresentInTheDataStore
        {
            public WhenAnEmployeeIsSearchedForAndIsPresentInTheDataStore()
            {
                _expectedEmployee = EmployeeFaker.Generate(1).Single();

                EmployeeStore
                    .Setup(s => s.FindEmployee(It.IsAny<Guid>()))
                    .ReturnsAsync(_expectedEmployee);

                var processor = new EmployeeProcessor(EmployeeStore.Object);
                _foundEmployee = processor.FindEmployee(_expectedEmployee.Id).Result;
            }

            [Fact]
            public void EmployeeIsFound()
                => _foundEmployee.Should().NotBeNull();

            [Fact]
            public void ExpectedEmployeeIsFound()
                => _foundEmployee.Should().Be(_expectedEmployee);

            [Fact]
            public void DataStoreIsCalledWithCorrectId()
                => EmployeeStore.Verify(e => e.FindEmployee(It.Is<Guid>(i => i == _expectedEmployee.Id)));
        }

        public class WhenAnEmployeeIsSearchedForAndIsNotPresentInTheDataStore
        {
            private readonly Guid _searchId;

            public WhenAnEmployeeIsSearchedForAndIsNotPresentInTheDataStore()
            {
                _searchId = Guid.NewGuid();
                var processor = new EmployeeProcessor(EmployeeStore.Object);
                _foundEmployee = processor.FindEmployee(_searchId).Result;
            }

            [Fact]
            public void EmployeeIsNotFound()
                => _foundEmployee.Should().BeNull();

            [Fact]
            public void DataStoreIsCalledWithCorrectId()
                => EmployeeStore.Verify(e => e.FindEmployee(It.Is<Guid>(i => i == _searchId)));
        }

    }
}
