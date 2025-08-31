using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class EmploymentTypeRepository : IEmploymentTypeRepository
    {
        private readonly JobApplicationSystemContext _context;

        public EmploymentTypeRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<EmploymentType> GetByIdAsync(int id)
        {
            return await _context.EmploymentTypes.FirstOrDefaultAsync(et => et.Id == id);
        }

        public async Task<EmploymentType?> GetByNameAsync(string typeName)
        {
            return await _context.EmploymentTypes.FirstOrDefaultAsync(et => et.TypeName.ToLower() == typeName.ToLower());
        }

        public async Task<IEnumerable<EmploymentType>> GetAllAsync()
        {
            return await _context.EmploymentTypes.ToListAsync();
        }

        public async Task AddAsync(EmploymentType employmentType)
        {
            if (employmentType == null)
            {
                throw new ArgumentNullException(nameof(employmentType));
            }
            await _context.EmploymentTypes.AddAsync(employmentType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmploymentType employmentType)
        {
            if (employmentType == null)
            {
                throw new ArgumentNullException(nameof(employmentType));
            }
            _context.EmploymentTypes.Update(employmentType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employmentType = await GetByIdAsync(id);
            if (employmentType == null)
            {
                throw new ArgumentNullException(nameof(employmentType), "EmploymentType not found");
            }
            _context.EmploymentTypes.Remove(employmentType);
            await _context.SaveChangesAsync();
        }
    }
}
