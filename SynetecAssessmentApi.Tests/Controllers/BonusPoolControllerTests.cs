using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Exceptions;
using SynetecAssessmentApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace SynetecAssessmentApi.Tests.Controllers
{
    public class BonusPoolControllerTests
    {
        [Fact]
        public void GetAllEmployeesTest()
        {
            var mocker = new AutoMocker();
            var mockBonusPoolService = new Mock<IBonusPoolService>();

            var allEmployees = new List<Dtos.EmployeeDto>
            {
                new Dtos.EmployeeDto
                {
                    Fullname = "Ms A",
                    Department = new Dtos.DepartmentDto()
                },
                new Dtos.EmployeeDto
                {
                    Fullname = "Mr B",
                    Department = new Dtos.DepartmentDto()
                }
            };

            mockBonusPoolService.Setup(m => m.GetEmployeesAsync()).ReturnsAsync(allEmployees);

            mocker.Use(mockBonusPoolService);

            var controller = mocker.CreateInstance<BonusPoolController>();

            var result = controller.GetAll().Result;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(allEmployees, okResult.Value);
            mockBonusPoolService.Verify(m => m.GetEmployeesAsync(), Times.Once);

        }

        [Fact]
        public void GetAllEmployeesExceptionTest()
        {
            var mocker = new AutoMocker();
            var mockBonusPoolService = new Mock<IBonusPoolService>();

            mockBonusPoolService.Setup(m => m.GetEmployeesAsync()).ThrowsAsync(new ExecutionEngineException());

            mocker.Use(mockBonusPoolService);
            var controller = mocker.CreateInstance<BonusPoolController>();

            var result = controller.GetAll().Result;

            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void CalculateBonusTest()
        {
            var mocker = new AutoMocker();
            var mockBonusPoolService = new Mock<IBonusPoolService>();

            var bonusPoolAmount = 100000;
            var employeeId = 1234;

            var calculateResult = new Dtos.BonusPoolCalculatorResultDto
            {
                Amount = 1500,
                Employee = new Dtos.EmployeeDto()
            };

            mockBonusPoolService.Setup(m => m.CalculateAsync(bonusPoolAmount, employeeId)).ReturnsAsync(calculateResult);
            mocker.Use(mockBonusPoolService);
            var controller = mocker.CreateInstance<BonusPoolController>();

            var result = controller.CalculateBonus(new Dtos.CalculateBonusDto { SelectedEmployeeId = employeeId, TotalBonusPoolAmount = bonusPoolAmount }).Result;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(calculateResult, okResult.Value);
            mockBonusPoolService.Verify(m => m.CalculateAsync(bonusPoolAmount, employeeId), Times.Once);

        }

        [Fact]
        public void CalculateBonusInvalidEmployeeIdTest()
        {
            var mocker = new AutoMocker();
            var mockBonusPoolService = new Mock<IBonusPoolService>();

            mockBonusPoolService.Setup(m => m.CalculateAsync(It.IsAny<int>(), It.IsAny<int>())).Throws(new InvalidEmployeeIdException(""));
            mocker.Use(mockBonusPoolService);

            var controller = mocker.CreateInstance<BonusPoolController>();

            var result = controller.CalculateBonus(new Dtos.CalculateBonusDto { SelectedEmployeeId = 54321, TotalBonusPoolAmount = 150000 }).Result;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            mockBonusPoolService.Verify(m => m.CalculateAsync(It.IsAny<int>(), It.IsAny<int>()));
        }
        [Fact]
        public void CalculateBonusExceptionTest()
        {
            var mocker = new AutoMocker();
            var mockBonusPoolService = new Mock<IBonusPoolService>();
            mockBonusPoolService.Setup(m => m.CalculateAsync(It.IsAny<int>(), It.IsAny<int>())).Throws(new ArgumentNullException(""));
            mocker.Use(mockBonusPoolService);

            var controller = mocker.CreateInstance<BonusPoolController>();

            var result = controller.CalculateBonus(new Dtos.CalculateBonusDto { SelectedEmployeeId = 54321, TotalBonusPoolAmount = 150000 }).Result;

            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}
