using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels.emp
{
    public class EmployeeRegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }
        
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Position Title")]
        public required string PositionTitle { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        
        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        
        [Display(Name = "City")]
        public string? City { get; set; }
        
        [Display(Name = "Country")]
        public string? Country { get; set; }
    }
}
