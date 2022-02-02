using System.Collections.Generic;
using System.Threading.Tasks;
using SynetecAssessmentApi.Dtos;

namespace SynetecAssessmentApi.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();
    }
}