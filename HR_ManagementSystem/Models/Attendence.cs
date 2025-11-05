using HR_ManagementSystem.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_ManagementSystem.Models
{
    public class Attendence
    {
        [ForeignKey("Employee")]
        public int EmpId { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly? Arrival { get; set; }
        public TimeOnly? Departure { get; set; }

        public AttendenceStatus Status { get; set; }

        public int? OvertimeInHours { get; set; }

        public int? LatetimeInHours { get; set; }
        
        public virtual Employee Employee { get; set; }
    }
}
