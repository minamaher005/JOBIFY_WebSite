using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IIndustryRepository
    {
        Task<Industry> GetByIdAsync(int id);
        Task<IEnumerable<Industry>> GetAllAsync();
        Task AddAsync(Industry industry);
        Task UpdateAsync(Industry industry);
        Task DeleteAsync(int id);
    }
}
