using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class JobLocation
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
