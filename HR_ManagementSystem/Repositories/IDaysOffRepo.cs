using HR_ManagementSystem.Models;

namespace HR_ManagementSystem.Repositories
{
    public interface IDaysOffRepo
    {
        public List<DaysOff> GetAll();
        public DaysOff GetByDay(DateOnly day);
        public void Add(DaysOff dayOff);
        public void Update(DateOnly day, DaysOff dayOff);
        public void Delete(DateOnly day);
    }
}
