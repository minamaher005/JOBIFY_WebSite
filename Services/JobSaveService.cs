using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class JobSaveService : ISavedJobsService
    {
        private readonly JobApplicationSystemContext _context;
        private readonly IJobRepository _jobRepository;
        private readonly IJobSeeker _jobSeekerRepository;
        private readonly ISavedJobRepository _savedJobRepository;

        public JobSaveService(
            JobApplicationSystemContext context,
            IJobRepository jobRepository,
            IJobSeeker jobSeekerRepository,
            ISavedJobRepository savedJobRepository)
        {
            _context = context;
            _jobRepository = jobRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _savedJobRepository = savedJobRepository;
        }

        public async Task<IEnumerable<SavedJob>> AddSavedJob(int jobSeekerId, int jobId)
        {
            // Check if job seeker exists
            var jobSeeker = await _jobSeekerRepository.GetByIdAsync(jobSeekerId);
            if (jobSeeker == null)
            {
                throw new KeyNotFoundException($"JobSeeker with ID {jobSeekerId} not found");
            }

            // Check if job exists
            var job = await _jobRepository.GetByIdAsync(jobId);
            if (job == null)
            {
                throw new KeyNotFoundException($"Job with ID {jobId} not found");
            }

            // Check if the job is already saved by this job seeker
            var existingSavedJob = await _context.SavedJobs
                .FirstOrDefaultAsync(sj => sj.JobId == jobId && sj.JobSeekerId == jobSeekerId);

            if (existingSavedJob != null)
            {
                throw new InvalidOperationException("This job is already saved");
            }

            // Create a new saved job
            var savedJob = new SavedJob
            {
                JobSeekerId = jobSeekerId,
                JobId = jobId,
                SavedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Add to database
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();

            // Return all saved jobs for this job seeker
            return await GetSavedJobsByJobSeekerId(jobSeekerId);
        }

        public async Task<IEnumerable<SavedJob>> GetSavedJobsByJobSeekerId(int jobSeekerId)
        {
            // Check if job seeker exists
            var jobSeeker = await _jobSeekerRepository.GetByIdAsync(jobSeekerId);
            if (jobSeeker == null)
            {
                throw new KeyNotFoundException($"JobSeeker with ID {jobSeekerId} not found");
            }

            // Get all saved jobs for this job seeker with related job information
            return await _context.SavedJobs
                .Where(sj => sj.JobSeekerId == jobSeekerId)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Company)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Location)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.EmploymentType)
                .ToListAsync();
        }

        public async Task<SavedJob> DeleteSavedJob(int jobSeekerId, int jobId)
        {
            // Find the saved job
            var savedJob = await _context.SavedJobs
                .FirstOrDefaultAsync(sj => sj.JobId == jobId && sj.JobSeekerId == jobSeekerId);

            if (savedJob == null)
            {
                throw new KeyNotFoundException("Saved job not found");
            }

            // Remove from database
            _context.SavedJobs.Remove(savedJob);
            await _context.SaveChangesAsync();

            return savedJob;
        }
    }
}