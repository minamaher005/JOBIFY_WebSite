using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class JobLocationRepository : IJobLocationRepository
    {
        private readonly JobApplicationSystemContext _context;

        public JobLocationRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<JobLocation> GetByIdAsync(int id)
        {
            return await _context.JobLocations.FirstOrDefaultAsync(jl => jl.Id == id);
        }

        public async Task<JobLocation?> GetByNameAsync(string locationName)
        {
            return await _context.JobLocations.FirstOrDefaultAsync(jl => jl.LocationName.ToLower() == locationName.ToLower());
        }

        public async Task<IEnumerable<JobLocation>> GetAllAsync()
        {
            return await _context.JobLocations.ToListAsync();
        }

        public async Task AddAsync(JobLocation jobLocation)
        {
            if (jobLocation == null)
            {
                throw new ArgumentNullException(nameof(jobLocation));
            }
            await _context.JobLocations.AddAsync(jobLocation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobLocation jobLocation)
        {
            if (jobLocation == null)
            {
                throw new ArgumentNullException(nameof(jobLocation));
            }
            _context.JobLocations.Update(jobLocation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobLocation = await GetByIdAsync(id);
            if (jobLocation == null)
            {
                throw new ArgumentNullException(nameof(jobLocation), "JobLocation not found");
            }
            _context.JobLocations.Remove(jobLocation);
            await _context.SaveChangesAsync();
        }
    }
}
