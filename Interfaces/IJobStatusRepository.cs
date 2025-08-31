using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IJobStatusRepository
    {
        Task<JobStatus> GetByIdAsync(int id);
        Task<JobStatus> GetByNameAsync(string statusName);
        Task<IEnumerable<JobStatus>> GetAllAsync();
        Task AddAsync(JobStatus jobStatus);
        Task UpdateAsync(JobStatus jobStatus);
        Task DeleteAsync(int id);
    }
}
