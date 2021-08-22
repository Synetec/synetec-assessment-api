using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Exceptions;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Persistence.Repositorys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public interface IBonusPoolService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();
        Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId);

    }
    public class BonusPoolService: IBonusPoolService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<BonusPoolService> _logger;

        public BonusPoolService(IEmployeeRepository employeeRepository, ILogger<BonusPoolService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployees();

            List<EmployeeDto> result = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                result.Add(
                    new EmployeeDto
                    {
                        Fullname = employee.Fullname,
                        JobTitle = employee.JobTitle,
                        Salary = employee.Salary,
                        Department = new DepartmentDto
                        {
                            Title = employee.Department.Title,
                            Description = employee.Department.Description
                        }
                    });
            }

            return result;
        }

        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            //load the details of the selected employee using the Id
            var employee = await _employeeRepository.GetEmployeeById(selectedEmployeeId);

            if (employee == null)
            {
                _logger.LogError($"Cannot find employee with Id {selectedEmployeeId}");
                throw new InvalidEmployeeIdException($"Invalid employee Id: {selectedEmployeeId}");
            }

            //get the total salary budget for the company
            int totalSalary = await _employeeRepository.GetTotalCompanySalary();

            //calculate the bonus allocation for the employee
            decimal bonusPercentage = (decimal)employee.Salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            return new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = employee.Fullname,
                    JobTitle = employee.JobTitle,
                    Salary = employee.Salary,
                    Department = new DepartmentDto
                    {
                        Title = employee.Department.Title,
                        Description = employee.Department.Description
                    }
                },

                Amount = bonusAllocation
            };
        }
    }
}
