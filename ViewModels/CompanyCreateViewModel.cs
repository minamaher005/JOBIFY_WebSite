using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.ViewModels
{
    public class CompanyCreateViewModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public required string Name { get; set; }
        
        [Display(Name = "Foundation Date")]
        [DataType(DataType.Date)]
        public DateOnly? FoundationDate { get; set; }
        
        [Required]
        [Display(Name = "Industry")]
        public required string IndustryName { get; set; }
        
        [Display(Name = "Website")]
        [Url]
        public string? Website { get; set; }
        
        [Display(Name = "Company Size")]
        public int? CompanySize { get; set; }
        
        [Display(Name = "Headquarters Location")]
        public string? HeadquartersLocation { get; set; }
        
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        
        [Display(Name = "Company Logo")]
        public IFormFile? LogoFile { get; set; }
        
        // For managing branches
        public List<BranchCreateViewModel> Branches { get; set; } = new List<BranchCreateViewModel>();
    }
}
