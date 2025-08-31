using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class SkillRepository : ISkillRepository
    {
        private readonly JobApplicationSystemContext _context;

        public SkillRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Skill> GetByIdAsync(int id)
        {
            return await _context.Skills.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<IEnumerable<Skill>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.JobSeekerSkills
                .Where(js => js.JobSeekerId == jobSeekerId)
                .Include(js => js.Skill)
                .Select(js => js.Skill)
                .ToListAsync();
        }

        public async Task AddAsync(Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var skill = await GetByIdAsync(id);
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill), "Skill not found");
            }
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
        }
    }
}
