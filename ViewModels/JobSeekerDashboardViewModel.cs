using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class JobSeekerDashboardViewModel
    {
        public JobSeeker JobSeeker { get; set; } = null!;
        
        public int SavedJobsCount { get; set; }
        
        public int AppliedJobsCount { get; set; }
        
        public List<Job> RecentJobs { get; set; } = new List<Job>();
        
        public List<Skill> Skills { get; set; } = new List<Skill>();
        
        // Profile completion percentage
        public int ProfileCompletionPercentage
        {
            get
            {
                int totalFields = 8;
                int completedFields = 0;
                
                if (JobSeeker.User != null)
                {
                    if (!string.IsNullOrEmpty(JobSeeker.User.FirstName)) completedFields++;
                    if (!string.IsNullOrEmpty(JobSeeker.User.LastName)) completedFields++;
                    if (!string.IsNullOrEmpty(JobSeeker.User.PhoneNumber)) completedFields++;
                }
                
                if (!string.IsNullOrEmpty(JobSeeker.Bio)) completedFields++;
                if (JobSeeker.YearsOfExperience.HasValue) completedFields++;
                if (JobSeeker.EducationLevelId.HasValue) completedFields++;
                if (!string.IsNullOrEmpty(JobSeeker.ProfilePictureUrl)) completedFields++;
                if (!string.IsNullOrEmpty(JobSeeker.ResumeUrl)) completedFields++;
                
                return (int)Math.Round((double)completedFields / totalFields * 100);
            }
        }
    }
}
