using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class EducationLevel
{
    public int Id { get; set; }

    public string LevelName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<JobSeeker> JobSeekers { get; set; } = new List<JobSeeker>();
}
