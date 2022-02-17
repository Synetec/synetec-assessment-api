using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using System;
using Xunit;

namespace SynetecAssessmentApi.Tests
{
    public class BonusPoolServiceTests
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Handle execution of contructor
        /// </summary>
        private static bool firstTry = true;

        /// <summary>
        /// Seed database
        /// </summary>
        public BonusPoolServiceTests()
        {
            if (firstTry)
            {
                //Validate database is only seeded once
                firstTry = false;

                var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");
                _dbContext = new AppDbContext(dbContextOptionBuilder.Options);
                DbContextGenerator.SeedData(_dbContext);
            }
        }


        [Fact]
        public void CalculateAsync_EmployeeNotInDb()
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

        /// <summary>
        /// The pole bonus is invalid
        /// </summary>
        [Fact]
        public void CalculateAsync_invalidPoleBonus()
        {
            var bonusPoolService = new BonusPoolService();

            //Arrange
            int bonusPoolAmount = -1020;
            int selectedEmployeeId = 2;

            //Act
            //Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => bonusPoolService.CalculateAsync(bonusPoolAmount, selectedEmployeeId));
            Assert.Equal("Pole bonus is not a valid value", exception.Result.Message);
        }


        //public void CalculateAsync_totalSalaryIsLessOrEqualToZero()
        //Will not fail because database is being filled hardcoded

        /// <summary>
        /// Calculate employee bonus
        /// </summary>
        [Fact]
        public async void CalculateAsync_EmployeeWithCorrectBonus()
        {
            var bonusPoolService = new BonusPoolService();

            //Arrange
            //Sum of all salaries on db
            int totalSalary = 654750;
            int bonusPoolAmount = 10000;
            int selectedEmployeeId = 2;
            int janetSalary = 90000;

            BonusPoolCalculatorResultDto expected = new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = "Janet Jones",
                    JobTitle = "HR Director",
                    Salary = janetSalary,
                    Department = new DepartmentDto
                    {
                        Title = "Human Resources",
                        Description = "The Human Resources department for the company"
                    }
                },

                Amount = (int)(bonusPoolAmount * ((decimal)janetSalary / (decimal)totalSalary))
            };

            //Act
            var employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, selectedEmployeeId);
            //Assert
            Assert.Equal(expected.Amount, employee.Amount);
        }
    }
}
