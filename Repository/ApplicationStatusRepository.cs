using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class ApplicationStatusRepository : IApplicationStatusRepository
    {
        private readonly JobApplicationSystemContext _context;

        public ApplicationStatusRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<ApplicationStatus> GetByIdAsync(int id)
        {
            return await _context.ApplicationStatuses.FirstOrDefaultAsync(ast => ast.Id == id);
        }

        public async Task<IEnumerable<ApplicationStatus>> GetAllAsync()
        {
            return await _context.ApplicationStatuses.ToListAsync();
        }

        public async Task AddAsync(ApplicationStatus applicationStatus)
        {
            if (applicationStatus == null)
            {
                throw new ArgumentNullException(nameof(applicationStatus));
            }
            await _context.ApplicationStatuses.AddAsync(applicationStatus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationStatus applicationStatus)
        {
            if (applicationStatus == null)
            {
                throw new ArgumentNullException(nameof(applicationStatus));
            }
            _context.ApplicationStatuses.Update(applicationStatus);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var applicationStatus = await GetByIdAsync(id);
            if (applicationStatus == null)
            {
                throw new ArgumentNullException(nameof(applicationStatus), "ApplicationStatus not found");
            }
            _context.ApplicationStatuses.Remove(applicationStatus);
            await _context.SaveChangesAsync();
        }
    }
}
