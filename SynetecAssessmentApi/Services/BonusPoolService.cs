using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService : IBonusPoolService
    {
        private readonly IAppDbContext _dbContext;

        public BonusPoolService(IAppDbContext dbContext)
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            _dbContext = dbContext;
        }

        /// <summary>
        /// Calculates bonous amount for selected employee with given bonous pool amount
        /// </summary>
        /// <param name="bonusPoolAmount"></param>
        /// <param name="selectedEmployeeId"></param>
        /// <returns></returns>
        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(decimal bonusPoolAmount, int selectedEmployeeId)
        {
            try
            {
                
                //load the details of the selected employee using the Id
                Employee employee = await _dbContext.Employees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(item => item.Id == selectedEmployeeId);

                //Handling if employee does not exist with given employee id
                if (employee == null)
                {
                    return new BonusPoolCalculatorResultDto
                    {
                        ErrorResponse = new ErrorResponseDTO
                        {
                            ErrorMessage = "Employee not found with given id.",
                            StatusCode = 400
                        }
                    };
                }

                //get the total salary budget for the company
                double totalSalary = (double)_dbContext.Employees.Sum(item => item.Salary);

                //calculate the bonus allocation for the employee
                decimal bonusPercentage = (decimal)employee.Salary / (decimal)totalSalary;
                double bonusAllocation = (double)(bonusPercentage * bonusPoolAmount);

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

                    Amount = Math.Round(bonusAllocation,2)
                };


            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
