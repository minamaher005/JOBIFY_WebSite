using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IEmploymentTypeRepository
    {
        Task<EmploymentType> GetByIdAsync(int id);
        Task<IEnumerable<EmploymentType>> GetAllAsync();
        Task<EmploymentType?> GetByNameAsync(string typeName);
        Task AddAsync(EmploymentType employmentType);
        Task UpdateAsync(EmploymentType employmentType);
        Task DeleteAsync(int id);
    }
}
