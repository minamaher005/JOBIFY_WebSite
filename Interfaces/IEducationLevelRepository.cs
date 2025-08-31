using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IEducationLevelRepository
    {
        Task<EducationLevel> GetByIdAsync(int id);
        Task<IEnumerable<EducationLevel>> GetAllAsync();
        Task AddAsync(EducationLevel educationLevel);
        Task UpdateAsync(EducationLevel educationLevel);
        Task DeleteAsync(int id);
    }
}
