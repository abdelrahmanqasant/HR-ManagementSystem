using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface IWeeklyDaysOff
    {
        public void Add(WeeklyDaysOff daysOff);
        public void Update(WeeklyDaysOff daysOff);

        public WeeklyDaysOff? Get();
    }
}
