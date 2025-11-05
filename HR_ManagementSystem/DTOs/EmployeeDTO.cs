using HR_ManagementSystem.Models;
using HR_ManagementSystem.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_ManagementSystem.DTOs
{
    public class EmployeeDTO
    {

        public int Id { get; set; }
        public string SSN { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }


        public string Nationality { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime ContractDate { get; set; }
        [Column(TypeName = "Money")]
        public decimal BaseSalary { get; set; }
        public TimeOnly Arrival { get; set; }
        public TimeOnly Departure { get; set; }

        public string DepartmentName { get; set; }
    }
}
