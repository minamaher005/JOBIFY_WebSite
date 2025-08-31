using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly JobApplicationSystemContext _context;

        public BranchRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _context.Branches
                .Include(b => b.Company)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches
                .Include(b => b.Company)
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> GetByCompanyAsync(int companyId)
        {
            return await _context.Branches
                .Where(b => b.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task AddAsync(Branch branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch));
            }
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch));
            }
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await GetByIdAsync(id);
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch), "Branch not found");
            }
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
        }
    }
}
