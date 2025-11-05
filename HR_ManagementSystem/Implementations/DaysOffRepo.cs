using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;

namespace HR_ManagementSystem.Implementations
{
    public class DaysOffRepo : IDaysOffRepo
    {
        private HRDbContext _context;
        public DaysOffRepo(HRDbContext context)
        {
            _context = context;
        }
        public void Add(DaysOff dayOff)
        {
            _context.DaysOffs.Add(dayOff);
        }

        public void Delete(DateOnly day)
        {
           _context.DaysOffs.Remove(GetByDay(day));
        }

        public List<DaysOff> GetAll()
        {
            return _context.DaysOffs.ToList();
        }

        public DaysOff GetByDay(DateOnly day)
        {
          return _context.DaysOffs.SingleOrDefault(i=>i.Date ==  day);
        }

        public void Update(DateOnly day, DaysOff dayOff)
        {
           var DayOff = GetByDay(day);
            if(dayOff != null)
            {
                _context.DaysOffs.Update(dayOff);
            }
        }
    }
}
