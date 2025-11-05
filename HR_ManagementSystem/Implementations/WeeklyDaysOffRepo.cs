using HR_ManagementSystem.Models;
using HR_ManagementSystem.Repositories;
using HR_ManagementSystem.Utilities;

namespace HR_ManagementSystem.Implementations
{
    public class WeeklyDaysOffRepo : IWeeklyDaysOff
    {
        private HRDbContext _context;
        public WeeklyDaysOffRepo(HRDbContext context)
        {
            _context = context;
        }

        public void Add(WeeklyDaysOff daysOff)
        {
            _context.WeeklyDaysOffs.Add(daysOff);
        }

        public WeeklyDaysOff? Get()
        {
            try
            {
                
                return new WeeklyDaysOff
                {
                    Id = 1,
                    Days = new List<DaysName>
            {
                DaysName.Friday,    
                DaysName.Saturday,  
               
            }
                };
            }
            catch (Exception ex)
            {
                return new WeeklyDaysOff
                {
                    Id = 1,
                    Days = new List<DaysName> { DaysName.Friday, DaysName.Saturday }
                };
            }
        }

        public void Update(WeeklyDaysOff daysOff)
        {
            _context.WeeklyDaysOffs.Update(daysOff);
        }
    }
}