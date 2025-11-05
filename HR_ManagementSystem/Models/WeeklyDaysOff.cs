using HR_ManagementSystem.Utilities;

namespace HR_ManagementSystem.Models
{
    public class WeeklyDaysOff
    {
        public int Id { get; set; }
        public List<DaysName> Days { get; set; } = new ();   
    }
}
