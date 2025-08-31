using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebApplication2.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly JobApplicationSystemContext _context;

        public JobRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Job> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.EmploymentType)
                .Include(j => j.Location)
                .Include(j => j.Company)
                .Include(j => j.JobStatus)
                .FirstOrDefaultAsync(j => j.Id == id) ?? throw new KeyNotFoundException($"Job with ID {id} not found");
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Include(j => j.EmploymentType)
                .Include(j => j.Location)
                .Include(j => j.Company)
                .Include(j => j.JobStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetByCompanyAsync(int companyId)
        {
            return await _context.Jobs
                .Where(j => j.CompanyId == companyId)
                .Include(j => j.EmploymentType)
                .Include(j => j.Location)
                .Include(j => j.JobStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetByJobStatusAsync(int jobStatusId)
        {
            return await _context.Jobs
                .Where(j => j.JobStatusId == jobStatusId)
                .Include(j => j.EmploymentType)
                .Include(j => j.Location)
                .Include(j => j.Company)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Job>> GetByLocationAsync(int locationId)
        {
            return await _context.Jobs
                .Where(j => j.LocationId == locationId)
                .Include(j => j.EmploymentType)
                .Include(j => j.Company)
                .Include(j => j.JobStatus)
                .ToListAsync();
        }
        
public async Task<IEnumerable<Job>> GetByEmployeeId(int employeeId)
        {
            return await _context.Jobs
                .Where(j => j.PostedByEmployeeId == employeeId)
                .Include(j => j.EmploymentType)
                .Include(j => j.Location)
                .Include(j => j.Company)
                .Include(j => j.JobStatus)
                .ToListAsync();
        }

        public async Task AddAsync(Job job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job job)
        {
            if (job == null)
            {
                throw new ArgumentNullException(nameof(job));
            }
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var job = await GetByIdAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
}