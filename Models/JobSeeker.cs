using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class JobSeeker
{
    public int Id { get; set; }

    public string? UserId { get; set; } // Foreign key to ApplicationUser

    public string FullName { get; set; } = null!;

    // Email removed as it's in ApplicationUser
    
    // PhoneNumber removed as it's in ApplicationUser

    public DateOnly? DateOfBirth { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Bio { get; set; }

    public int? YearsOfExperience { get; set; }

    public string? UniversityName { get; set; }

    public int? EducationLevelId { get; set; }

    public string? ResumeUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? PortfolioUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual EducationLevel? EducationLevel { get; set; }

    public virtual ICollection<JobSeekerSkill> JobSeekerSkills { get; set; } = new List<JobSeekerSkill>();

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    
    public virtual ApplicationUser? User { get; set; } // Navigation property to ApplicationUser
}
