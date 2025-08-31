using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public Employee Employee { get; set; } = null!;
        
        public int ActiveJobsCount { get; set; }
        
        public int TotalApplicationsCount { get; set; }
        
        public int NewApplicationsCount { get; set; }
        
        public List<Job> RecentJobs { get; set; } = new List<Job>();
    }
}
