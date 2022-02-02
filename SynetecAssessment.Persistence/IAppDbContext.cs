using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;

namespace SynetecAssessmentApi.Persistence
{
    public interface IAppDbContext
    {
        DbSet<Department> Departments { get; set; }
        DbSet<Employee> Employees { get; set; }
    }
}