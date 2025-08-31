
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebApplication2.Repository
{
    public class JobSeekerRepository : IJobSeeker
    {
        private readonly JobApplicationSystemContext _context;

        public JobSeekerRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<JobSeeker> GetByIdAsync(int id)
        {
            return await _context.JobSeekers.FirstOrDefaultAsync(js => js.Id == id);
        }

        public async Task<IEnumerable<JobSeeker>> GetAllAsync()
        {
            return await _context.JobSeekers.ToListAsync();
        }

        public async Task AddAsync(JobSeeker jobSeeker)
        {
            if (jobSeeker == null)
            {
                throw new ArgumentNullException(nameof(jobSeeker));
            }
            _context.JobSeekers.Add(jobSeeker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeeker jobSeeker)
        {
            if (jobSeeker == null)
            {
                throw new ArgumentNullException(nameof(jobSeeker));
            }
            _context.JobSeekers.Update(jobSeeker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobSeeker = await GetByIdAsync(id);
            if (jobSeeker == null) throw new ArgumentNullException(nameof(jobSeeker));
            _context.JobSeekers.Remove(jobSeeker);
            await _context.SaveChangesAsync();
        }

    }
}