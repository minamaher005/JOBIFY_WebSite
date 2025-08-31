using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Services.Application_service
{
    public class JobApplyService : IJobApplyService
    {
        private readonly JobApplicationSystemContext _context;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IJobSeekerService _jobSeekerService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobApplyService(
            JobApplicationSystemContext context,
            IApplicationRepository applicationRepository,
            IJobRepository jobRepository,
            IJobSeekerService jobSeekerService,
            IEmployeeRepository employeeRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _jobSeekerService = jobSeekerService;
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Application> CreateApplicationAsync(string userId, ApplicationCreateViewModel model)
        {
            try
            {
                // Find the job seeker with the given userId
                var jobSeeker = await _context.JobSeekers
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                {
                    throw new KeyNotFoundException($"JobSeeker with User ID {userId} not found");
                }

                // Get the job
                var job = await _jobRepository.GetByIdAsync(model.JobId);
                if (job == null)
                {
                    throw new KeyNotFoundException($"Job with ID {model.JobId} not found");
                }

                // Check if the job seeker has already applied
                var existingApplication = await _context.Applications
                    .AnyAsync(a => a.JobId == model.JobId && a.JobSeekerId == jobSeeker.Id);

                if (existingApplication)
                {
                    throw new InvalidOperationException("You have already applied for this job");
                }

                // Check if ApplicationStatuses table is populated
                var statuses = await _context.ApplicationStatuses.ToListAsync();
                if (!statuses.Any())
                {
                    // Create application statuses if they don't exist
                    var newStatuses = new List<ApplicationStatus>
                    {
                        new ApplicationStatus 
                        { 
                            Id = 1,
                            StatusName = "Pending", 
                            CreatedAt = DateTime.UtcNow, 
                            UpdatedAt = DateTime.UtcNow 
                        },
                        new ApplicationStatus 
                        { 
                            Id = 2,
                            StatusName = "Reviewed", 
                            CreatedAt = DateTime.UtcNow, 
                            UpdatedAt = DateTime.UtcNow 
                        },
                        new ApplicationStatus 
                        { 
                            Id = 3,
                            StatusName = "Interview", 
                            CreatedAt = DateTime.UtcNow, 
                            UpdatedAt = DateTime.UtcNow 
                        },
                        new ApplicationStatus 
                        { 
                            Id = 4,
                            StatusName = "Accepted", 
                            CreatedAt = DateTime.UtcNow, 
                            UpdatedAt = DateTime.UtcNow 
                        },
                        new ApplicationStatus 
                        { 
                            Id = 5,
                            StatusName = "Rejected", 
                            CreatedAt = DateTime.UtcNow, 
                            UpdatedAt = DateTime.UtcNow 
                        }
                    };
                    
                    await _context.ApplicationStatuses.AddRangeAsync(newStatuses);
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine("Added ApplicationStatuses data");
                }
                else
                {
                    Console.WriteLine($"Found {statuses.Count} application statuses. Available IDs: {string.Join(", ", statuses.Select(s => s.Id))}");
                }

                // Handle resume file upload
                string? resumeUrl = null;
                if (model.Resume != null)
                {
                    // Create directory if it doesn't exist
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "resumes");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique filename
                    string uniqueFileName = $"user_{userId}_{DateTime.Now.Ticks}_{model.Resume.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Resume.CopyToAsync(fileStream);
                    }

                    // Store the relative URL
                    resumeUrl = $"/uploads/resumes/{uniqueFileName}";
                }

                // Get the correct status ID for "Pending" (should be 1)
                var pendingStatus = await _context.ApplicationStatuses
                    .FirstOrDefaultAsync(s => s.StatusName == "Pending");
                
                if (pendingStatus == null)
                {
                    // Create a new pending status if it doesn't exist
                    pendingStatus = new ApplicationStatus
                    {
                        StatusName = "Pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    await _context.ApplicationStatuses.AddAsync(pendingStatus);
                    await _context.SaveChangesAsync();
                }

                // Create a new application - only map fields that exist in the Application model
                var application = new Application
                {
                    JobId = model.JobId,
                    JobSeekerId = jobSeeker.Id,
                    StatusId = pendingStatus.Id, // Use the retrieved status ID
                    PreviousCompany = model.PreviousCompany,
                    ResumeUrl = resumeUrl,
                    AppliedDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Add the application to the repository
                await _applicationRepository.AddAsync(application);

                return application;
            }
            catch (Exception ex)
            {
                // Capture the detailed error message
                Console.WriteLine($"Error creating application: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw the exception for the controller to handle
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId)
        {
            // Get all applications for a specific job
            return await _context.Applications
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.User)
                .Include(a => a.Status)
                .Where(a => a.JobId == jobId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsForEmployeeAsync(string userId)
        {
            // Get the employee
            var employee = await _employeeRepository.GetByUserIdAsync(userId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with User ID {userId} not found");
            }

            // Get jobs posted by this employee
            var jobIds = await _context.Jobs
                .Where(j => j.PostedByEmployeeId == employee.Id)
                .Select(j => j.Id)
                .ToListAsync();

            // Get applications for these jobs
            return await _context.Applications
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.User)
                .Include(a => a.Job)
                .Include(a => a.Status)
                .Where(a => jobIds.Contains(a.JobId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByJobSeekerAsync(string userId)
        {
            // Get the job seeker
            var jobSeeker = await _context.JobSeekers
                .FirstOrDefaultAsync(js => js.UserId == userId);

            if (jobSeeker == null)
            {
                throw new KeyNotFoundException($"JobSeeker with User ID {userId} not found");
            }

            // Get all applications for this job seeker
            return await _context.Applications
                .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                .Include(a => a.Job)
                    .ThenInclude(j => j.Location)
                .Include(a => a.Status)
                .Where(a => a.JobSeekerId == jobSeeker.Id)
                .ToListAsync();
        }

        public async Task<Application> UpdateApplicationStatusAsync(int applicationId, int newStatusId, string employeeUserId)
        {
            // Get the application
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                throw new KeyNotFoundException($"Application with ID {applicationId} not found");
            }

            // Get the employee
            var employee = await _employeeRepository.GetByUserIdAsync(employeeUserId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with User ID {employeeUserId} not found");
            }

            // Get the job to verify the employee is authorized to update this application
            var job = await _jobRepository.GetByIdAsync(application.JobId);

            if (job.PostedByEmployeeId != employee.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this application");
            }

            // Update the application status
            application.StatusId = newStatusId;
            application.UpdatedAt = DateTime.Now;

            // Save the changes
            await _applicationRepository.UpdateAsync(application);

            return application;
        }

        public async Task<Application> GetApplicationByIdAsync(int applicationId)
        {
            var application = await _context.Applications
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.User)
                .Include(a => a.Job)
                    .ThenInclude(j => j.Company)
                .Include(a => a.Job)
                    .ThenInclude(j => j.PostedByEmployee)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
            {
                throw new KeyNotFoundException($"Application with ID {applicationId} not found");
            }

            return application;
        }

        public async Task<bool> HasJobSeekerAppliedToJobAsync(int jobId, string jobSeekerUserId)
        {
            // Get the job seeker
            var jobSeeker = await _context.JobSeekers
                .FirstOrDefaultAsync(js => js.UserId == jobSeekerUserId);

            if (jobSeeker == null)
            {
                return false;
            }

            // Check if the job seeker has already applied
            return await _context.Applications
                .AnyAsync(a => a.JobId == jobId && a.JobSeekerId == jobSeeker.Id);
        }
    }
}
