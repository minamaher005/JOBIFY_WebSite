using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IJobRepository
    {
        Task<Job> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync();
        Task<IEnumerable<Job>> GetByCompanyAsync(int companyId);
        Task<IEnumerable<Job>> GetByJobStatusAsync(int jobStatusId);
        Task<IEnumerable<Job>> GetByLocationAsync(int locationId);
        Task<IEnumerable<Job>> GetByEmployeeId(int employeeId);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(int id);
    }
}
