namespace HR_ManagementSystem.DTOs
{
    public class PermissionFormDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<CheckBoxDTO> RoleClaims { get; set; }
    }
}
