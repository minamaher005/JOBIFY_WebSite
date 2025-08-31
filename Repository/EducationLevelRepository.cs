using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class EducationLevelRepository : IEducationLevelRepository
    {
        private readonly JobApplicationSystemContext _context;

        public EducationLevelRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<EducationLevel> GetByIdAsync(int id)
        {
            return await _context.EducationLevels.FirstOrDefaultAsync(el => el.Id == id);
        }

        public async Task<IEnumerable<EducationLevel>> GetAllAsync()
        {
            return await _context.EducationLevels.ToListAsync();
        }

        public async Task AddAsync(EducationLevel educationLevel)
        {
            if (educationLevel == null)
            {
                throw new ArgumentNullException(nameof(educationLevel));
            }
            await _context.EducationLevels.AddAsync(educationLevel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EducationLevel educationLevel)
        {
            if (educationLevel == null)
            {
                throw new ArgumentNullException(nameof(educationLevel));
            }
            _context.EducationLevels.Update(educationLevel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var educationLevel = await GetByIdAsync(id);
            if (educationLevel == null)
            {
                throw new ArgumentNullException(nameof(educationLevel), "EducationLevel not found");
            }
            _context.EducationLevels.Remove(educationLevel);
            await _context.SaveChangesAsync();
        }
    }
}
