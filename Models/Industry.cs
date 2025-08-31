using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Industry
{
    public int Id { get; set; }

    public string IndustryType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
