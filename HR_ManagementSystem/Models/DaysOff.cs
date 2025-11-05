using System.ComponentModel.DataAnnotations;

namespace HR_ManagementSystem.Models
{
    public class DaysOff
    {
        [Key]
        public DateOnly Date { get; set; }
        public string Name { get; set; }
    }
}
