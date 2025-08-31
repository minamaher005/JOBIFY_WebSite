using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.ViewModels
{
    public class JobSeekerEditViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }
        
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Bio")]
        [DataType(DataType.MultilineText)]
        public string? Bio { get; set; }
        
        [Display(Name = "Years of Experience")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int? YearsOfExperience { get; set; }
        
        [Display(Name = "Education Level")]
        public int? EducationLevelId { get; set; }
        
        [Display(Name = "LinkedIn URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? LinkedInUrl { get; set; }
        
        [Display(Name = "Portfolio URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? PortfolioUrl { get; set; }
        
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        [Display(Name = "Current Profile Picture")]
        public string? CurrentProfilePictureUrl { get; set; }
        
        [Display(Name = "Resume")]
        public IFormFile? Resume { get; set; }
        
        [Display(Name = "Current Resume")]
        public string? CurrentResumeUrl { get; set; }
        
        [Display(Name = "Skills")]
        public List<int>? SelectedSkills { get; set; }
        
        [Display(Name = "Skills (comma separated)")]
        public string? ManualSkills { get; set; }
        
        public List<SelectListItem>? AvailableSkills { get; set; }
    }
}