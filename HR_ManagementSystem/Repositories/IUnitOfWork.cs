namespace HR_ManagementSystem.Repositories
{
    public interface IUnitOfWork
    {
        IAttendence Attendence { get; }
        ICommission Commission { get; }

        IDeduction deduction { get; }

        IDaysOffRepo daysOffRepo { get; }
        IWeeklyDaysOff weeklyDaysOffRepo { get; }
        IDepartmentRepo departmentRepo { get; }
        IEmployeeRepo employeeRepo { get; } 

        Task<int> SaveChangesAsync();
    }
}
