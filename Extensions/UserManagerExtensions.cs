using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<Employee> GetEmployeeAsync(
            this UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            JobApplicationSystemContext dbContext)
        {
            if (user == null)
                return null;

            // Find the employee associated with this user
            return await dbContext.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);
        }
    }
}
