using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IJobLocationRepository
    {
        Task<JobLocation> GetByIdAsync(int id);
        Task<IEnumerable<JobLocation>> GetAllAsync();
        Task<JobLocation?> GetByNameAsync(string locationName);
        Task AddAsync(JobLocation jobLocation);
        Task UpdateAsync(JobLocation jobLocation);
        Task DeleteAsync(int id);
    }
}
