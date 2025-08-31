using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
