using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface ISavedJobRepository
    {
        Task<SavedJob> GetByIdAsync(int id);
        Task<IEnumerable<SavedJob>> GetAllAsync();
        Task<IEnumerable<SavedJob>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<IEnumerable<SavedJob>> GetByJobIdAsync(int jobId);
        Task AddAsync(SavedJob savedJob);
        Task DeleteAsync(int id);
    }
}
