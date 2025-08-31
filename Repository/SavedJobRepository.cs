using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class SavedJobRepository : ISavedJobRepository
    {
        private readonly JobApplicationSystemContext _context;

        public SavedJobRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<SavedJob> GetByIdAsync(int id)
        {
            // Since SavedJob has composite key (JobSeekerId, JobId), we'll use a different approach
            // In practice, you'd likely pass both keys to identify a SavedJob
            // For now, assuming 'id' is the primary key of the SavedJob's JobId for simplicity
            return await _context.SavedJobs
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Company)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Location)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.EmploymentType)
                .Include(sj => sj.JobSeeker)
                .FirstOrDefaultAsync(sj => sj.JobId == id);
        }

        public async Task<IEnumerable<SavedJob>> GetAllAsync()
        {
            return await _context.SavedJobs
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Company)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Location)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.EmploymentType)
                .Include(sj => sj.JobSeeker)
                .ToListAsync();
        }

        public async Task<IEnumerable<SavedJob>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.SavedJobs
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Company)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.Location)
                .Include(sj => sj.Job)
                    .ThenInclude(j => j.EmploymentType)
                .Include(sj => sj.JobSeeker)
                .Where(sj => sj.JobSeekerId == jobSeekerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SavedJob>> GetByJobIdAsync(int jobId)
        {
            return await _context.SavedJobs
                .Include(sj => sj.Job)
                .Include(sj => sj.JobSeeker)
                .Where(sj => sj.JobId == jobId)
                .ToListAsync();
        }

        public async Task AddAsync(SavedJob savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Since SavedJob has composite key, we need both keys to identify a record
            // For simplicity, assuming 'id' is the JobId
            var savedJob = await _context.SavedJobs.FirstOrDefaultAsync(sj => sj.JobId == id);
            if (savedJob != null)
            {
                _context.SavedJobs.Remove(savedJob);
                await _context.SaveChangesAsync();
            }
        }
    }
}
