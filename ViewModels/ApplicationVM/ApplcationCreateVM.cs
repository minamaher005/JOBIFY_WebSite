using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebApplication2.ViewModels.ApplicationVM
{
    public class ApplcationCreateVM
    {
        // The job ID that the job seeker is applying for
        [Required]
        public int JobId { get; set; }
        
        // Information about the job (to display on the application form)
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? JobLocation { get; set; }
        
        // Optional previous company information
        [Display(Name = "Previous Company")]
        public string? PreviousCompany { get; set; }
        
        // Cover letter for the application
        [Display(Name = "Cover Letter")]
        [DataType(DataType.MultilineText)]
        public string? CoverLetter { get; set; }
        
        // The date of application (will be set to current date by default)
        [Display(Name = "Applied Date")]
        [DataType(DataType.Date)]
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        
        // Any additional information to include with the application
        [Display(Name = "Additional Information")]
        [DataType(DataType.MultilineText)]
        public string? AdditionalInformation { get; set; }
        
        // A flag to allow the job seeker to use their existing resume or upload a new one
        [Display(Name = "Use Existing Resume")]
        public bool UseExistingResume { get; set; } = true;
        
        // If a new resume is to be uploaded
        [Display(Name = "Upload New Resume")]
        public Microsoft.AspNetCore.Http.IFormFile? ResumeFile { get; set; }
    }
}