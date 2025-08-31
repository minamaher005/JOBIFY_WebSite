using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class JobStatusRepository : IJobStatusRepository
    {
        private readonly JobApplicationSystemContext _context;

        public JobStatusRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<JobStatus> GetByIdAsync(int id)
        {
            return await _context.JobStatuses.FirstOrDefaultAsync(js => js.Id == id);
        }

        public async Task<JobStatus> GetByNameAsync(string statusName)
        {
            return await _context.JobStatuses.FirstOrDefaultAsync(js => js.StatusName == statusName);
        }

        public async Task<IEnumerable<JobStatus>> GetAllAsync()
        {
            return await _context.JobStatuses.ToListAsync();
        }

        public async Task AddAsync(JobStatus jobStatus)
        {
            if (jobStatus == null)
            {
                throw new ArgumentNullException(nameof(jobStatus));
            }
            await _context.JobStatuses.AddAsync(jobStatus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobStatus jobStatus)
        {
            if (jobStatus == null)
            {
                throw new ArgumentNullException(nameof(jobStatus));
            }
            _context.JobStatuses.Update(jobStatus);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobStatus = await GetByIdAsync(id);
            if (jobStatus == null)
            {
                throw new ArgumentNullException(nameof(jobStatus), "JobStatus not found");
            }
            _context.JobStatuses.Remove(jobStatus);
            await _context.SaveChangesAsync();
        }
    }
}
