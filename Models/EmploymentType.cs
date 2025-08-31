using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class EmploymentType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
