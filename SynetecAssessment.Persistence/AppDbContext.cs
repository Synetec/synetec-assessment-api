using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using System.Reflection;

namespace SynetecAssessmentApi.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
