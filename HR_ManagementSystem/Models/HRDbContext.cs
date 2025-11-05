using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR_ManagementSystem.Models
{
    public class HRDbContext :IdentityDbContext<ApplicationUser> 
    {
        public HRDbContext() {  }


        public HRDbContext(DbContextOptions<HRDbContext> options) :base(options) { }
        
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Attendence> Attendences { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<DaysOff> DaysOffs { get; set; }
        public virtual DbSet<CommissionSettings> CommissionSettings { get; set; }
        public virtual DbSet<DeductionSettings> DeductionSettings { get; set; }
        public virtual DbSet<WeeklyDaysOff> WeeklyDaysOffs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Attendence>()
                .HasKey(k => new { k.EmpId, k.Day });
            builder.Entity<Department>()
                .HasIndex(d =>d.Name).IsUnique();
            builder.Entity<Employee>()
               .HasIndex(e => e.SSN).IsUnique();
            builder.Entity<Employee>()
                .HasIndex(e =>e.PhoneNumber).IsUnique();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        
    }
}
