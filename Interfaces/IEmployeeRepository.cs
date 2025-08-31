using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
namespace WebApplication2.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetByUserIdAsync(string userId);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByRoleIdAsync(int roleId);
        Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
