using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;

namespace SynetecAssessmentApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IAppDbContext _dbContext;

        public EmployeeService(IAppDbContext dbContext)
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all emplooyee details.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            try
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
            catch(Exception)
            {
                throw;
            }
            
        }
    }
}
