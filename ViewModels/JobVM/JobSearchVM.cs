using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebApplication2.Models;
using WebApplication2.ViewModels;   
namespace WebApplication2.ViewModels
{
    public class JobSearchVM
    {
        
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = null!;

        
        [Display(Name = "Job Location")]
        public string LocationName { get; set; } = null!;

        public string EmploymentType { get; set; }

        public int? RequiredExperienceYears { get; set; }

        [Display(Name = "Salary From")]
        public decimal? SalaryFrom { get; set; }

        [Display(Name = "Salary To")]
        public decimal? SalaryTo { get; set; }

        [Display(Name = "Currency")]
        public string? Currency { get; set; }
        
    }
}