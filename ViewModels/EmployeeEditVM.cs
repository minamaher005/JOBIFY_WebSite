using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.ViewModels
{
    public class EmployeeEditViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }
        
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }
        
        [Display(Name = "City")]
        public string? City { get; set; }
        
        [Display(Name = "Country")]
        public string? Country { get; set; }
        
        [Display(Name = "Bio")]
        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }
        
        [Required]
        [Display(Name = "Position Title")]
        public required string PositionTitle { get; set; }
        
        [Required]
        [Display(Name = "Date Joined")]
        [DataType(DataType.Date)]
        public DateOnly DateJoined { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        
        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        [Display(Name = "Current Profile Picture")]
        public string? CurrentProfilePictureUrl { get; set; }
        
        // For dropdowns
        public List<SelectListItem>? AvailableRoles { get; set; }
        public List<SelectListItem>? AvailableCompanies { get; set; }
    }
}
