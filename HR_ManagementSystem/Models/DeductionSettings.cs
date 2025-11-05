using HR_ManagementSystem.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_ManagementSystem.Models
{
    public class DeductionSettings
    {
        public int Id { get; set; }
        public Unit type { get; set; }
        public int Hours { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
    }
}
