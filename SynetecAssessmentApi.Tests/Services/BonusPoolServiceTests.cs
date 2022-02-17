using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace SynetecAssessmentApi.Tests
{
    public class BonusPoolServiceTests : BonusPoolService
    {
        [Fact]
        public void CalculateAsync_EmployeeIsNull()
        {
            var bonusPoolService = new BonusPoolService();

            //Arrange
            int bonusPoolAmount = 0;
            int selectedEmployeeId = 0;

            //Act
            //Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bonusPoolService.CalculateAsync(bonusPoolAmount, selectedEmployeeId));
            Assert.Equal(exception.Result.Message, "Employee with ID " + selectedEmployeeId.ToString() + " not found");
        }


        //public void CalculateAsync_totalSalaryIsLessOrEqualToZero()
        //Will not fail because database is being filled hardcoded

        protected override void setDbContext(AppDbContext dbContext)
        {
            base.setDbContext(dbContext);
        }

        [Fact]
        public async void CalculateAsync_EmployeeWithCorrectBonus()
        {
            var bonusPoolService = new BonusPoolService();

            //Arrange
            int bonusPoolAmount = 10000;
            int selectedEmployeeId = 2;
            int totalSalary = 0;

            var departments = new List<Department>
            {
                new Department(1, "Finance", "The finance department for the company"),
                new Department(2, "Human Resources", "The Human Resources department for the company"),
                new Department(3, "IT", "The IT support department for the company"),
                new Department(4, "Marketing", "The Marketing department for the company")
            };

            var employees = new List<Employee>
            {
                new Employee(1, "John Smith", "Accountant (Senior)", 60000, 1),
                new Employee(2, "Janet Jones", "HR Director", 90000, 2),
                new Employee(3, "Robert Rinser", "IT Director", 95000, 3),
                new Employee(4, "Jilly Thornton", "Marketing Manager (Senior)", 55000, 4),
                new Employee(5, "Gemma Jones", "Marketing Manager (Junior)", 45000, 4),
                new Employee(6, "Peter Bateman", "IT Support Engineer", 35000, 3),
                new Employee(7, "Azimir Smirkov", "Creative Director", 62500, 4),
                new Employee(8, "Penelope Scunthorpe", "Creative Assistant", 38750, 4),
                new Employee(9, "Amil Kahn", "IT Support Engineer", 36000, 3),
                new Employee(10, "Joe Masters", "IT Support Engineer", 36500, 3),
                new Employee(11, "Paul Azgul", "HR Manager", 53000, 2),
                new Employee(12, "Jennifer Smith", "Accountant (Junior)", 48000, 1),
            };

            this._dbContext.Employees.AddRange(employees);

            this._dbContext.SaveChanges();

            setDbContext(this._dbContext);

            BonusPoolCalculatorResultDto expected = new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = "Janet Jones",
                    JobTitle = "HR Director",
                    Salary = 90000,
                    Department = new DepartmentDto
                    {
                        Title = "Human Resources",
                        Description = "The Human Resources department for the company"
                    }
                },

                Amount = 5000
            };

            //Act
            var employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, selectedEmployeeId);
            //Assert
            Assert.Equal(employee, expected);
        }
    }
}
