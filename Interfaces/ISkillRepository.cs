using WebApplication2.Models;
namespace WebApplication2.Interfaces
{


    public interface ISkillRepository
    {
        Task<Skill> GetByIdAsync(int id);
        Task<IEnumerable<Skill>> GetAllAsync();
        Task<IEnumerable<Skill>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task AddAsync(Skill skill);
        Task UpdateAsync(Skill skill);
        Task DeleteAsync(int id);
    }
    
    }
