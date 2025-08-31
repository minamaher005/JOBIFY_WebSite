using WebApplication2.Models;
namespace WebApplication2.Interfaces
{
    
    
    public interface ICompanyREpository
    {
        Task<Company> GetByIdAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task DeleteAsync(int id);
    }
   
    }
