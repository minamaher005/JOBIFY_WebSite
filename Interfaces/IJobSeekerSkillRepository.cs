using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IJobSeekerSkillRepository
    {
        Task<JobSeekerSkill> GetByIdAsync(int jobSeekerId, int skillId);
        Task<IEnumerable<JobSeekerSkill>> GetAllAsync();
        Task<IEnumerable<JobSeekerSkill>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<IEnumerable<JobSeekerSkill>> GetBySkillIdAsync(int skillId);
        Task AddAsync(JobSeekerSkill jobSeekerSkill);
        Task UpdateAsync(JobSeekerSkill jobSeekerSkill);
        Task DeleteAsync(int jobSeekerId, int skillId);
    }
}
