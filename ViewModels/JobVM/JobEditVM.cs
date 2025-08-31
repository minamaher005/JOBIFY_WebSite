using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class JobEditVM
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;
        
        [Required]
        [Display(Name = "Job Location")]
        public int LocationId { get; set; }
        
        [Required]
        [Display(Name = "Employment Type")]
        public int EmploymentTypeId { get; set; }
        
        [Display(Name = "Years of Experience")]
        public int? RequiredExperienceYears { get; set; }
        
        [Display(Name = "Salary From")]
        [DataType(DataType.Currency)]
        public decimal? SalaryFrom { get; set; }
        
        [Display(Name = "Salary To")]
        [DataType(DataType.Currency)]
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
        
        [Required]
        [Display(Name = "Job Status")]
        public int JobStatusId { get; set; }
    }
}