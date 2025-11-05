using System.ComponentModel.DataAnnotations;

namespace HR_ManagementSystem.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Full Name Is Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required!"),EmailAddress(ErrorMessage ="Please Enter A Valid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Password And ConfirmPassword Must Be The Same")]
        public string ConfirmPassword { get; set; }

        public string RoleName {  get; set; }
    }
}
