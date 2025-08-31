using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class JobViewEmpVM
    {
        public int Id { get; set; }
        
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;
        
        [Display(Name = "Company")]
        public string CompanyName { get; set; } = null!;
        
        [Display(Name = "Years of Experience")]
        public int? RequiredExperienceYears { get; set; }
        
        [Display(Name = "Status")]
        public string JobStatus { get; set; } = null!;
        
        [Display(Name = "Posted Date")]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }
        
        [Display(Name = "Applications")]
        public int ApplicationCount { get; set; }
    }
}