using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class BranchCreateViewModel
    {
        [Required(ErrorMessage = "Branch location is required")]
        [Display(Name = "Branch Location")]
        public string BranchLocation { get; set; } = string.Empty;
        
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? PhoneNumber { get; set; }
    }
}
