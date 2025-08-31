using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.ViewModels
{
    public class JobSeekerRegisterViewModel
    {
        // Identity-related fields (for ApplicationUser)
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
        
        // JobSeeker profile fields
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
        
        [Display(Name = "Years of Experience")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50")]
        public int? YearsOfExperience { get; set; }
        
        [Display(Name = "University")]
        public string? UniversityName { get; set; }
        
        [Display(Name = "Education Level")]
        public int? EducationLevelId { get; set; }
        
        [Display(Name = "LinkedIn URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? LinkedInUrl { get; set; }
        
        [Display(Name = "Portfolio URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? PortfolioUrl { get; set; }
        
        // File uploads
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        [Display(Name = "Resume")]
        public IFormFile? Resume { get; set; }
        
        // Skills
        [Display(Name = "Skills")]
        public List<int>? SelectedSkills { get; set; }
        
        [Display(Name = "Skills (comma separated)")]
        public string? ManualSkills { get; set; }
        
        public List<SelectListItem>? AvailableSkills { get; set; }
    }
}
