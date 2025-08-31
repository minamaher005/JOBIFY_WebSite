using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class JobStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
