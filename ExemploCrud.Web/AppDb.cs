using ExemploCrud.Web.Models;
using Microsoft.EntityFrameworkCore;


namespace ExemploCrud.Web
{
    public class AppDb : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public AppDb( DbContextOptions<AppDb> options) : 
            base(options) { }

        protected override void 
            OnConfiguring(DbContextOptionsBuilder dbBuilder)
        {
            if(!dbBuilder.IsConfigured ) {
                dbBuilder.UseSqlite("Data Source=employee.db");
            }

        }
    }
}
