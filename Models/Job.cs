using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Job
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int? PostedByEmployeeId { get; set; }

    public int JobStatusId { get; set; }

    public int EmploymentTypeId { get; set; }

    public int? RequiredExperienceYears { get; set; }

    public string JobTitle { get; set; } = null!;

    public int LocationId { get; set; }

    public decimal? SalaryFrom { get; set; }

    public decimal? SalaryTo { get; set; }

    public string? Currency { get; set; }

    public DateOnly? ApplicationDeadline { get; set; }

    public string? Requirements { get; set; }

    public string? Benefits { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Company Company { get; set; } = null!;

    public virtual EmploymentType EmploymentType { get; set; } = null!;

    public virtual JobStatus JobStatus { get; set; } = null!;

    public virtual JobLocation Location { get; set; } = null!;

    public virtual Employee? PostedByEmployee { get; set; }

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
}
