using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly JobApplicationSystemContext _context;

        public IndustryRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Industry> GetByIdAsync(int id)
        {
            return await _context.Industries.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Industry>> GetAllAsync()
        {
            return await _context.Industries.ToListAsync();
        }

        public async Task AddAsync(Industry industry)
        {
            if (industry == null)
            {
                throw new ArgumentNullException(nameof(industry));
            }
            await _context.Industries.AddAsync(industry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Industry industry)
        {
            if (industry == null)
            {
                throw new ArgumentNullException(nameof(industry));
            }
            _context.Industries.Update(industry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var industry = await GetByIdAsync(id);
            if (industry == null)
            {
                throw new ArgumentNullException(nameof(industry), "Industry not found");
            }
            _context.Industries.Remove(industry);
            await _context.SaveChangesAsync();
        }
    }
}
