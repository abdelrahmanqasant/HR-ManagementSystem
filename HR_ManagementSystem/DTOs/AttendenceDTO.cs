namespace HR_ManagementSystem.DTOs
{
    public class AttendenceDTO
    {
        public int EmpId { get; set; }

  
        public string Day { get; set; }

     
        public string? Arrival { get; set; }
        public string? Departure { get; set; }

        public int Status { get; set; }
        public string? DeptName { get; set; }
        public string? EmpName { get; set; }
    }
}
