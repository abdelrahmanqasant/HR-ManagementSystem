using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly HRDbContext _context;

    public IAttendence Attendence { get; }
    public ICommission Commission { get; }
    public IDeduction deduction { get; }
    public IDaysOffRepo daysOffRepo { get; }
    public IWeeklyDaysOff weeklyDaysOffRepo { get; }  
    public IDepartmentRepo departmentRepo { get; }
    public IEmployeeRepo employeeRepo { get; }

    public UnitOfWork(
        HRDbContext context,
        IAttendence attendence,
        ICommission commission,
        IDeduction deduction,
        IDaysOffRepo daysOffRepo,
        IWeeklyDaysOff weeklyDaysOffRepo,  
        IDepartmentRepo departmentRepo,
        IEmployeeRepo employeeRepo
        )
    {
        _context = context;
        this.Attendence = attendence;
        this.Commission = commission;
        this.deduction = deduction;
        this.daysOffRepo = daysOffRepo;
        this.weeklyDaysOffRepo = weeklyDaysOffRepo;  
        this.departmentRepo = departmentRepo;
        this.employeeRepo = employeeRepo;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}