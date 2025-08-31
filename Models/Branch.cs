using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Branch
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string BranchLocation { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;
}
