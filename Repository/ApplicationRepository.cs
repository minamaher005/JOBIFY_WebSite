using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebApplication2.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly JobApplicationSystemContext _context;
        public ApplicationRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }
        public async Task<Application> GetByIdAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.JobSeeker)
                .Include(a => a.Status)
                .FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Application with ID {id} not found");
        }
        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.JobSeeker)
                .Include(a => a.Status)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Applications
                .Where(a => a.JobSeekerId == jobSeekerId)
                .Include(a => a.Job)
                .Include(a => a.Status)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobId == jobId)
                .Include(a => a.JobSeeker)
                .Include(a => a.Status)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Application>> GetByStatusIdAsync(int statusId)
        {
            return await _context.Applications
                .Where(a => a.StatusId == statusId)
                .Include(a => a.Job)
                .Include(a => a.JobSeeker)
                .ToListAsync();
        }
        
        public async Task AddAsync(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateAsync(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var application = await GetByIdAsync(id);
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application), "Application not found");
            }
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }
      

        
   
    } 
}