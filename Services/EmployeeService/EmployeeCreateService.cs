using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Services.EmployeeService
{
    public class EmployeeCreateService : IEmployeeServices
    {
        private readonly JobApplicationSystemContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileUploadService _fileUploadService;

        public EmployeeCreateService(
            JobApplicationSystemContext context,
            UserManager<ApplicationUser> userManager,
            IFileUploadService fileUploadService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
        }

        public async Task CreateEmployeeAsync(EmployeeCreateViewModel employeeVM)
        {
            if (employeeVM == null)
            {
                throw new ArgumentNullException(nameof(employeeVM));
            }

            var employee = new Employee
            {
                FullName = employeeVM.FirstName + " " + employeeVM.LastName,
                DateOfBirth = employeeVM.DateOfBirth,
                City = employeeVM.City,
                Country = employeeVM.Country,
                Bio = employeeVM.Bio,
                RoleId = employeeVM.RoleId,
                CompanyId = employeeVM.CompanyId,
                PositionTitle = employeeVM.PositionTitle,
                DateJoined = employeeVM.DateJoined,
                IsActive = employeeVM.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Handle profile picture upload if provided
            if (employeeVM.ProfilePicture != null && employeeVM.ProfilePicture.Length > 0)
            {
                // Implement file upload logic similar to JobSeekerService
                // Assuming you have a FileUploadService injected
                employee.ProfilePictureUrl = await _fileUploadService.UploadFileAsync(
                    employeeVM.ProfilePicture,
                    "employee-photos",
                    $"employee_{Guid.NewGuid()}");
            }

            // Create user account for the employee
            var user = new ApplicationUser
            {
                UserName = employeeVM.Email,
                Email = employeeVM.Email,
                PhoneNumber = employeeVM.PhoneNumber,
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName
            };

            var result = await _userManager.CreateAsync(user, employeeVM.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user account: {errors}");
            }

            // Assign the user to the Employee role
            await _userManager.AddToRoleAsync(user, "Employee");

            // Link the employee to the user account
            employee.UserId = user.Id;

            // Save the employee to the database
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task EditEmployeeAsync(int id, EmployeeEditViewModel employeeVM)
        {
            if (employeeVM == null)
            {
                throw new ArgumentNullException(nameof(employeeVM));
            }

            var employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found");
            }

            if (employee.User == null)
            {
                throw new KeyNotFoundException($"User account for employee with ID {id} not found");
            }

            // Update user properties
            employee.User.PhoneNumber = employeeVM.PhoneNumber;
            employee.User.FirstName = employeeVM.FirstName;
            employee.User.LastName = employeeVM.LastName;

            // Update employee properties
            employee.FullName = $"{employeeVM.FirstName} {employeeVM.LastName}";
            employee.DateOfBirth = employeeVM.DateOfBirth;
            employee.City = employeeVM.City;
            employee.Country = employeeVM.Country;
            employee.Bio = employeeVM.Bio;
            employee.RoleId = employeeVM.RoleId;
            employee.CompanyId = employeeVM.CompanyId;
            employee.PositionTitle = employeeVM.PositionTitle;
            employee.DateJoined = employeeVM.DateJoined;
            employee.IsActive = employeeVM.IsActive;
            employee.UpdatedAt = DateTime.UtcNow;

            // Handle profile picture update
            if (employeeVM.ProfilePicture != null && employeeVM.ProfilePicture.Length > 0)
            {
                // Delete existing profile picture if any
                if (!string.IsNullOrEmpty(employee.ProfilePictureUrl))
                {
                    _fileUploadService.DeleteFile(employee.ProfilePictureUrl);
                }

                // Upload new profile picture
                employee.ProfilePictureUrl = await _fileUploadService.UploadFileAsync(
                    employeeVM.ProfilePicture,
                    "employee-photos",
                    $"employee_{employee.UserId}");
            }

            // Save changes
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}