using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.ViewModels
{
    public class ApplicationCreateViewModel
    {
        [Required]
        public int JobId { get; set; }
        
        [Display(Name = "Previous Company")]
        [StringLength(200, ErrorMessage = "Previous company name cannot exceed 200 characters.")]
        public string? PreviousCompany { get; set; }
        
        [Required]
        [Display(Name = "Resume/CV")]
        public IFormFile Resume { get; set; } = null!;
        
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Available Start Date")]
        [DataType(DataType.Date)]
        public DateOnly? AvailableStartDate { get; set; }
        
        [Display(Name = "Expected Salary")]
        [DataType(DataType.Currency)]
        public decimal? ExpectedSalary { get; set; }
        
        [Display(Name = "Additional Information")]
        [StringLength(2000, ErrorMessage = "Additional information cannot exceed 2000 characters.")]
        public string? AdditionalInformation { get; set; }
    }
}
