using HR_ManagementSystem.DTOs;

namespace HR_ManagementSystem.Models
{
    public class OrganizationSettings
    {

        public CommissionDTO? CommissionDTO { get; set; }
        public DeductionDTO? DeductionDTO { get; set; }
        public WeeklyDaysDTO? WeeklyDaysDTO { get; set; }
    }
}
