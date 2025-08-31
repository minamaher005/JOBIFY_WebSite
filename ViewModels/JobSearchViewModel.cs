using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class JobSearchViewModel
    {
        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
        public string? SearchTerm { get; set; }
        public int? LocationId { get; set; }
        public int? EmploymentTypeId { get; set; }
    }
}
