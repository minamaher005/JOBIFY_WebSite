using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly JobApplicationSystemContext _context;

        public RoleRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task AddAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role not found");
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
