using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService
    {
        private readonly AppDbContext _dbContext;

        public BonusPoolService()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            _dbContext = new AppDbContext(dbContextOptionBuilder.Options);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            IEnumerable<Employee> employees = await _dbContext
                .Employees
                .Include(e => e.Department)
                .ToListAsync();

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

        public bool isBonusValid(CalculateBonusDto request)
        {
            bool isBonusValid = false;

            if(request.TotalBonusPoolAmount >= 0)
            {
                isBonusValid = true;
            }

            return isBonusValid;
        }

        public bool isBonusValid(int bonusPoolAmount)
        {
            bool isBonusValid = false;

            if (bonusPoolAmount >= 0)
            {
                isBonusValid = true;
            }

            return isBonusValid;
        }

        /// <summary>
        /// Calculates Bonus for the selected employee
        /// </summary>
        /// <param name="bonusPoolAmount"></param>
        /// <param name="selectedEmployeeId"></param>
        /// <returns></returns>
        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            if(isBonusValid(bonusPoolAmount))
            {
                //load the details of the selected employee using the Id
                Employee employee = await _dbContext.Employees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(item => item.Id == selectedEmployeeId);

                if (employee == null)
                {
                    throw new ArgumentException("Employee with ID " + selectedEmployeeId.ToString() + " not found");
                }

                //get the total salary budget for the company
                int totalSalary = (int)_dbContext.Employees.Sum(item => item.Salary);

                //Precaution: In case the database is not seeded or seeded with negative values
                if (totalSalary <= 0)
                {
                    throw new Exception("Sum of salaries is incorrect");
                }

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
            else
            {
                throw new ArgumentException("Total bonus is not a valid value");
            }
            
        }
    }
}
