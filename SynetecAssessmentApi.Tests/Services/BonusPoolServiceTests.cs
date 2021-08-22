using Moq;
using Moq.AutoMock;
using SynetecAssessmentApi.Persistence.Repositorys;
using SynetecAssessmentApi.Services;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Exceptions;

namespace SynetecAssessmentApi.Tests.Services
{
    
    public class BonusPoolServiceTests
    {
        [Fact]
        public void GetAllEmployeesTest()
        {
            var mocker = new AutoMocker();

            var mockRepository = new Mock<IEmployeeRepository>();

            var employees = new List<Domain.Employee>
            {
                new Employee(123, "ABC", "Teaboy", 15000,666){Department = new Department(666, "Tea Making", "Make the tea")},
                new Employee(456, "DEF", "Reserve Teaboy", 14500,666){Department = new Department(666, "Tea Making", "Make the tea")}
            };

            mockRepository.Setup(m => m.GetAllEmployees()).ReturnsAsync(employees);

            mocker.Use(mockRepository);

            var service = mocker.CreateInstance<BonusPoolService>();

            var results = service.GetEmployeesAsync().Result;

            Assert.NotNull(results.First(m => m.Fullname == "ABC"));
            Assert.NotNull(results.First(m => m.Fullname == "DEF"));

            mockRepository.Verify(m => m.GetAllEmployees(), Times.Once);
        }

        [Fact]
        public async void CalculateInvalidEmployeeTest()
        {
            var mocker = new AutoMocker();

            var mockRepository = new Mock<IEmployeeRepository>();
            Employee employee = null;
            mockRepository.Setup(m => m.GetEmployeeById(1)).ReturnsAsync(employee);
            mocker.Use(mockRepository);

            var service = mocker.CreateInstance<BonusPoolService>();

            await Assert.ThrowsAsync<InvalidEmployeeIdException>(() => service.CalculateAsync(100000, 12345));
        }

        [Fact]
        public void CalculateTest()
        {
            var mocker = new AutoMocker();

            var mockRepository = new Mock<IEmployeeRepository>();

            var employee = new Employee(123, "Mr 15k", "Title", 15000, 666) { Department = new Department(666, "Title", "Desc") };
            mockRepository.Setup(m => m.GetEmployeeById(123)).ReturnsAsync(employee);
            mockRepository.Setup(m => m.GetTotalCompanySalary()).ReturnsAsync(100000);
            mocker.Use(mockRepository);

            var service = mocker.CreateInstance<BonusPoolService>();

            var result = service.CalculateAsync(10000, 123).Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Employee);
            Assert.Equal(1500, result.Amount);
        }
    }
}
