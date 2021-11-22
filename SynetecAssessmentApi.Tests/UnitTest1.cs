using System;
using SynetecAssessmentApi.Services;
using SynetecAssessmentApi.Controllers;
using Xunit;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// xUnit test project created for unit testing of individual employee bonus created. 
/// Challanges for this 
/// This was first time I was writting the unit test cases for data that has been seed using seed method. 
/// So initally loading the data to run the test cases took some time for me figure it out. 
/// There is scope for this code to be improved. 
/// </summary>
namespace SynetecAssessmentApi.Tests
{
    public class BonusPoolTestCase
    {
        private readonly AppDbContext _dbContext;

        static int i = 0;
        public BonusPoolTestCase()
        {
            if (i == 0)
            {
                var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
                dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");
                _dbContext = new AppDbContext(dbContextOptionBuilder.Options);
                DbContextGenerator.SeedData(_dbContext);
                i++;
            }
        }

        [Fact]
        public async void BonusPool100KEmpID1()
        {
            //Arrange 
            int employeeID = 1;
            int salary = 60000;
            double totalSalary = 654750;
            int bonusPoolAmount = 100000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService b = new BonusPoolService();
            BonusPoolCalculatorResultDto Employee = await b.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, Employee.Amount);
        }

        [Fact]
        public async void BonusPool100KEmpID2()
        {
            //Arrange 
            int employeeID = 2;
            int salary = 90000;
            double totalSalary = 654750;
            int bonusPoolAmount = 100000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, employee.Amount);
        }

        [Fact]
        public async void BonusPool100KEmpID3()
        {
            //Arrange 
            int employeeID = 3;
            int salary = 95000;
            double totalSalary = 654750;
            int bonusPoolAmount = 100000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, employee.Amount);
        }


        [Fact]
        public async void BonusPool250KEmpID1()
        {
            //Arrange 
            int employeeID = 1;
            int salary = 60000;
            double totalSalary = 654750;
            int bonusPoolAmount = 250000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, employee.Amount);
        }

        [Fact]
        public async void BonusPool250KEmpID2()
        {
            //Arrange 
            int employeeID = 2;
            int salary = 90000;
            double totalSalary = 654750;
            int bonusPoolAmount = 250000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, employee.Amount);
        }

        [Fact]
        public async void BonusPool250KEmpID3()
        {
            //Arrange 
            int employeeID = 3;
            int salary = 95000;
            double totalSalary = 654750;
            int bonusPoolAmount = 250000;
            decimal bonusPercentage = (decimal)salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            //Act
            BonusPoolService bonusPoolService = new BonusPoolService();
            BonusPoolCalculatorResultDto employee = await bonusPoolService.CalculateAsync(bonusPoolAmount, employeeID);

            //Assert
            Assert.Equal(bonusAllocation, employee.Amount);
        }
    }
}
