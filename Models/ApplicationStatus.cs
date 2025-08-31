using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class ApplicationStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
