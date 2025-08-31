using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class JobSeekerSkill
{
    public int JobSeekerId { get; set; }

    public int SkillId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual JobSeeker JobSeeker { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
