using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
