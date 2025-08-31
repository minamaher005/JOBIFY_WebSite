using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication2.Models;
using WebApplication2.ViewModels;   
namespace WebApplication2.ViewModels
{
    public class JobCreateVM
    {
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;
        
        [Required]
        [Display(Name = "Job Location")]
        public int LocationId { get; set; }
        
        [Display(Name = "Location Text")]
        public string? LocationText { get; set; }
        
        [Required]
        [Display(Name = "Employment Type")]
        public int EmploymentTypeId { get; set; }
        
        [Display(Name = "Employment Type Text")]
        public string? EmploymentTypeText { get; set; }
        
        [Display(Name = "Required Experience (Years)")]
        public int? RequiredExperienceYears { get; set; }
        
        [Display(Name = "Salary From")]
        public decimal? SalaryFrom { get; set; }
        
        [Display(Name = "Salary To")]
        public decimal? SalaryTo { get; set; }
        
        [Display(Name = "Currency")]
        public string? Currency { get; set; }
        
        [Display(Name = "Application Deadline")]
        [DataType(DataType.Date)]
        public DateOnly? ApplicationDeadline { get; set; }
        
        [Display(Name = "Requirements")]
        [DataType(DataType.MultilineText)]
        public string? Requirements { get; set; }
        
        [Display(Name = "Benefits")]
        [DataType(DataType.MultilineText)]
        public string? Benefits { get; set; }
    }
}