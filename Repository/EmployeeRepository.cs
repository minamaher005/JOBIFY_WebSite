using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebApplication2.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly JobApplicationSystemContext _context;

        public EmployeeRepository(JobApplicationSystemContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Company)
                .Include(e => e.User)  // Include User information
                .FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException($"Employee with ID {id} not found");
        }
        
        public async Task<Employee> GetByUserIdAsync(string userId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.UserId == userId) ?? throw new KeyNotFoundException($"Employee with User ID {userId} not found");
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Company)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByRoleIdAsync(int roleId)
        {
            return await _context.Employees
                .Where(e => e.RoleId == roleId)
                .Include(e => e.Company)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Employees
                .Where(e => e.CompanyId == companyId)
                .Include(e => e.Role)
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}