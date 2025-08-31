using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string SkillName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<JobSeekerSkill> JobSeekerSkills { get; set; } = new List<JobSeekerSkill>();
}
