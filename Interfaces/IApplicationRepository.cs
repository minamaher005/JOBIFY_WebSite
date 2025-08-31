using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
namespace WebApplication2.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application> GetByIdAsync(int id);
        Task<IEnumerable<Application>> GetAllAsync();
        Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
        Task<IEnumerable<Application>> GetByStatusIdAsync(int statusId);
        Task AddAsync(Application application);
        Task UpdateAsync(Application application);
        Task DeleteAsync(int id);
    }
}