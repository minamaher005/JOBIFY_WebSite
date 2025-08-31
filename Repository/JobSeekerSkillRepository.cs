using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class JobSeekerSkillRepository : IJobSeekerSkillRepository
    {
        private readonly JobApplicationSystemContext _context;

        public JobSeekerSkillRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerSkill> GetByIdAsync(int jobSeekerId, int skillId)
        {
            return await _context.JobSeekerSkills
                .FirstOrDefaultAsync(js => js.JobSeekerId == jobSeekerId && js.SkillId == skillId);
        }

        public async Task<IEnumerable<JobSeekerSkill>> GetAllAsync()
        {
            return await _context.JobSeekerSkills
                .Include(js => js.JobSeeker)
                .Include(js => js.Skill)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerSkill>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.JobSeekerSkills
                .Where(js => js.JobSeekerId == jobSeekerId)
                .Include(js => js.Skill)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerSkill>> GetBySkillIdAsync(int skillId)
        {
            return await _context.JobSeekerSkills
                .Where(js => js.SkillId == skillId)
                .Include(js => js.JobSeeker)
                .ToListAsync();
        }

        public async Task AddAsync(JobSeekerSkill jobSeekerSkill)
        {
            if (jobSeekerSkill == null)
            {
                throw new ArgumentNullException(nameof(jobSeekerSkill));
            }
            await _context.JobSeekerSkills.AddAsync(jobSeekerSkill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeekerSkill jobSeekerSkill)
        {
            if (jobSeekerSkill == null)
            {
                throw new ArgumentNullException(nameof(jobSeekerSkill));
            }
            _context.JobSeekerSkills.Update(jobSeekerSkill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int jobSeekerId, int skillId)
        {
            var jobSeekerSkill = await GetByIdAsync(jobSeekerId, skillId);
            if (jobSeekerSkill == null)
            {
                throw new ArgumentNullException(nameof(jobSeekerSkill), "JobSeekerSkill not found");
            }
            _context.JobSeekerSkills.Remove(jobSeekerSkill);
            await _context.SaveChangesAsync();
        }
    }
}
