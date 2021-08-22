using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;

namespace SynetecAssessmentApi.Persistence.Repositorys
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int EmployeeId);
        Task<int> GetTotalCompanySalary();

    }
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;
        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _dbContext
                .Employees
                .Include(e => e.Department)
                .ToListAsync();
        }
        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(item => item.Id == employeeId);
        }

        public async Task<int> GetTotalCompanySalary() 
        {
            return _dbContext.Employees.Sum(item => item.Salary);
        }
    }
}
