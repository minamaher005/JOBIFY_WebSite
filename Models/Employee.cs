using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Employee
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

    public int RoleId { get; set; }

    public int CompanyId { get; set; }

    public string PositionTitle { get; set; } = null!;

    public DateOnly DateJoined { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual Role Role { get; set; } = null!;
    
    public virtual ApplicationUser? User { get; set; } // Navigation property to ApplicationUser
}
