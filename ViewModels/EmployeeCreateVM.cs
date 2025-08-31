using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.ViewModels
{
    public class EmployeeCreateViewModel
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
        
        // Employee profile fields
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
        public DateOnly DateJoined { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
        
        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        
        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        
        // File upload
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        // For dropdowns
        public List<SelectListItem>? AvailableRoles { get; set; }
        public List<SelectListItem>? AvailableCompanies { get; set; }
    }
}