using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebApplication2.Repository
{
    public class CompanyRepository : ICompanyREpository
    {
        private readonly JobApplicationSystemContext _context;

        public CompanyRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }
        public async Task<Company> GetByIdAsync(int id)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }
        public async Task AddAsync(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var company = await GetByIdAsync(id);
            if (company == null) throw new ArgumentNullException(nameof(company));
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        

    }
}