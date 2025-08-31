using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IApplicationStatusRepository
    {
        Task<ApplicationStatus> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationStatus>> GetAllAsync();
        Task AddAsync(ApplicationStatus applicationStatus);
        Task UpdateAsync(ApplicationStatus applicationStatus);
        Task DeleteAsync(int id);
    }
}
