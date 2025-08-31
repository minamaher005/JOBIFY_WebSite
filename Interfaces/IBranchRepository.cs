using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IBranchRepository
    {
        Task<Branch> GetByIdAsync(int id);
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<IEnumerable<Branch>> GetByCompanyAsync(int companyId);
        Task AddAsync(Branch branch);
        Task UpdateAsync(Branch branch);
        Task DeleteAsync(int id);
    }
}
