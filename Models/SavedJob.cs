using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class SavedJob
{
    public int JobSeekerId { get; set; }

    public int JobId { get; set; }

    public DateTime? SavedDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual JobSeeker JobSeeker { get; set; } = null!;
}
