using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class CompanyCreateVM
    {
        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Display(Name = "Foundation Date")]
        [DataType(DataType.Date)]
        public DateOnly? FoundationDate { get; set; }

        [Required]
        [Display(Name = "Industry")]
        public int IndustryId { get; set; }

        [Display(Name = "Website")]
        [Url]
        public string Website { get; set; }

        [Display(Name = "Company Size")]
        public int? CompanySize { get; set; }

        [Display(Name = "Headquarters Location")]
        public string HeadquartersLocation { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        // Branch information
        [Required]
        [Display(Name = "Branch Location")]
        public string BranchLocation { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        // Additional branches (optional)
        public List<BranchCreateVM> AdditionalBranches { get; set; } = new List<BranchCreateVM>();
    }

    public class BranchCreateVM
    {
        [Required]
        [Display(Name = "Branch Location")]
        public string BranchLocation { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
