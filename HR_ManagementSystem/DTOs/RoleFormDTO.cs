using System.ComponentModel.DataAnnotations;

namespace HR_ManagementSystem.DTOs
{
    public class RoleFormDTO
    {
        [Required,MaxLength(256)]

        public string Name { get; set; }
    }
}
