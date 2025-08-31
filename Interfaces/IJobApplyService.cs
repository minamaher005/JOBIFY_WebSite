using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.ViewModels.ApplicationVM;

namespace WebApplication2.Interfaces
{
    public interface IJobApplyService
    {
        // Create a new application
        Task<Application> CreateApplicationAsync(string userId, ApplicationCreateViewModel model);
        
        // Get all applications for a job
        Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId);
        
        // Get all applications posted by a specific employee (based on jobs they posted)
        Task<IEnumerable<Application>> GetApplicationsForEmployeeAsync(string userId);
        
        // Get all applications for a job seeker
        Task<IEnumerable<Application>> GetApplicationsByJobSeekerAsync(string userId);
        
        // Update application status
        Task<Application> UpdateApplicationStatusAsync(int applicationId, int newStatusId, string employeeUserId);
        
        // Get application details by ID
        Task<Application> GetApplicationByIdAsync(int applicationId);
        
        // Check if a job seeker has already applied to a job
        Task<bool> HasJobSeekerAppliedToJobAsync(int jobId, string jobSeekerUserId);
    }
}