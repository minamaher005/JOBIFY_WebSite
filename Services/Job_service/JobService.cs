using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Services.Job_service
{
    public class JobService : IJobCreateService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly JobApplicationSystemContext _context;

        public JobService(IJobRepository jobRepository, IEmployeeRepository employeeRepository, JobApplicationSystemContext context)
        {
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;
            _context = context;
        }

        public async Task<Job> CreateJobAsync(string userId, JobCreateVM jobVM)
        {
            // Find the employee with the given userId
            var employee = await _context.Employees
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with User ID {userId} not found");
            }

            if (employee.CompanyId <= 0)
            {
                throw new InvalidOperationException("Employee must be associated with a company to create job postings");
            }

            // Create a new job entity from the view model
            var job = new Job
            {
                JobTitle = jobVM.JobTitle,
                LocationId = jobVM.LocationId,
                EmploymentTypeId = jobVM.EmploymentTypeId,
                RequiredExperienceYears = jobVM.RequiredExperienceYears,
                SalaryFrom = jobVM.SalaryFrom,
                SalaryTo = jobVM.SalaryTo,
                Currency = jobVM.Currency,
                ApplicationDeadline = jobVM.ApplicationDeadline,
                Requirements = jobVM.Requirements,
                Benefits = jobVM.Benefits,
                
                // Set the related properties
                CompanyId = employee.CompanyId,
                PostedByEmployeeId = employee.Id,
                PostedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Find or create an "Active" job status
            var activeStatus = await _context.JobStatuses.FirstOrDefaultAsync(s => s.StatusName == "Active");
            if (activeStatus == null)
            {
                // Create a new "Active" status if it doesn't exist
                activeStatus = new JobStatus
                {
                    StatusName = "Active",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _context.JobStatuses.AddAsync(activeStatus);
                await _context.SaveChangesAsync();
            }
            
            job.JobStatusId = activeStatus.Id;

            // Add the job to the repository
            await _jobRepository.AddAsync(job);

            return job;
        }

        public async Task<Job> EditJobAsync(int id, JobEditVM jobVM)
        {
            // Get the existing job
            var job = await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {id} not found");
            }

            // Update the job properties
            job.JobTitle = jobVM.JobTitle;
            job.LocationId = jobVM.LocationId;
            job.EmploymentTypeId = jobVM.EmploymentTypeId;
            job.RequiredExperienceYears = jobVM.RequiredExperienceYears;
            job.SalaryFrom = jobVM.SalaryFrom;
            job.SalaryTo = jobVM.SalaryTo;
            job.Currency = jobVM.Currency;
            job.ApplicationDeadline = jobVM.ApplicationDeadline;
            job.Requirements = jobVM.Requirements;
            job.Benefits = jobVM.Benefits;
            job.JobStatusId = jobVM.JobStatusId;
            job.UpdatedAt = DateTime.Now;

            // Save the changes
            await _jobRepository.UpdateAsync(job);

            return job;
        }

        public async Task<Job> deleteJobAsync(int id)
        {
            // Get the job to be deleted
            var job = await _jobRepository.GetByIdAsync(id);

            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {id} not found");
            }

            // Delete the job
            await _jobRepository.DeleteAsync(id);

            return job;
        }
    }
}
