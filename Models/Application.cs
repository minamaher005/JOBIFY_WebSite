using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Application
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int StatusId { get; set; }

    public int JobSeekerId { get; set; }

    public string? PreviousCompany { get; set; }

    public string? ResumeUrl { get; set; }

    public DateTime? AppliedDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual JobSeeker JobSeeker { get; set; } = null!;

    public virtual ApplicationStatus Status { get; set; } = null!;
    
}