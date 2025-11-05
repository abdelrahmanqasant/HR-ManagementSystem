namespace HR_ManagementSystem.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
