using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? FoundationDate { get; set; }

    public int IndustryId { get; set; }

    public string? Website { get; set; }

    public int? CompanySize { get; set; }

    public string? HeadquartersLocation { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public decimal? Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Industry Industry { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
