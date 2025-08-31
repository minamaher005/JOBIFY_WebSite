using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyREpository _companyRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IEmployeeServices _employeeService;
        private readonly JobApplicationSystemContext _context;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IRoleRepository roleRepository,
            ICompanyREpository companyRepository,
            IBranchRepository branchRepository,
            IIndustryRepository industryRepository,
            IEmployeeServices employeeService,
            JobApplicationSystemContext context)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            _industryRepository = industryRepository;
            _employeeService = employeeService;
            _context = context;
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            // Load roles for dropdown
            var roles = await _roleRepository.GetAllAsync();
            
            // Load companies for dropdown
            var companies = await _companyRepository.GetAllAsync();
            
            var model = new EmployeeCreateViewModel
            {
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                PositionTitle = string.Empty,
                DateJoined = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                AvailableRoles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RoleName
                }).ToList(),
                AvailableCompanies = companies.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            
            return View(model);
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            // Check if we have a newly created company from TempData
            if (TempData["NewCompanyId"] != null && int.TryParse(TempData["NewCompanyId"].ToString(), out int newCompanyId))
            {
                model.CompanyId = newCompanyId;
                ModelState.ClearValidationState("CompanyId");
                ModelState.MarkFieldValid("CompanyId");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.CreateEmployeeAsync(model);
                    return RedirectToAction("Index", "Home", new { message = "Employee registration successful!" });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred during registration: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            // Reload roles and companies for dropdowns
            var roles = await _roleRepository.GetAllAsync();
            var companies = await _companyRepository.GetAllAsync();
            
            model.AvailableRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoleName,
                Selected = r.Id == model.RoleId
            }).ToList();
            
            model.AvailableCompanies = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CompanyId
            }).ToList();
            
            return View(model);
        }
        
        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Get employee with User information included
            var employee = await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Company)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
                
            if (employee == null)
                return NotFound();
                
            if (employee.User == null)
                return NotFound($"User account for employee with ID {id} not found");
            
            // Load roles for dropdown
            var roles = await _roleRepository.GetAllAsync();
            
            // Load companies for dropdown
            var companies = await _companyRepository.GetAllAsync();
            
            var model = new EmployeeEditViewModel
            {
                FirstName = employee.User.FirstName ?? string.Empty,
                LastName = employee.User.LastName ?? string.Empty,
                PhoneNumber = employee.User.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                City = employee.City,
                Country = employee.Country,
                Bio = employee.Bio,
                RoleId = employee.RoleId,
                CompanyId = employee.CompanyId,
                PositionTitle = employee.PositionTitle,
                DateJoined = employee.DateJoined,
                IsActive = employee.IsActive ?? true,
                CurrentProfilePictureUrl = employee.ProfilePictureUrl,
                AvailableRoles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RoleName,
                    Selected = r.Id == employee.RoleId
                }).ToList(),
                AvailableCompanies = companies.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == employee.CompanyId
                }).ToList()
            };
            
            return View(model);
        }
        
        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.EditEmployeeAsync(id, model);
                    return RedirectToAction("Dashboard", "Employee", new { message = "Employee profile updated successfully!" });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while updating the employee profile: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            // Reload roles and companies for dropdowns
            var roles = await _roleRepository.GetAllAsync();
            var companies = await _companyRepository.GetAllAsync();
            
            model.AvailableRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoleName,
                Selected = r.Id == model.RoleId
            }).ToList();
            
            model.AvailableCompanies = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CompanyId
            }).ToList();
            
            return View(model);
        }
        
        // GET: Employee/Dashboard
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }

            // Get employee from the user ID
            var employee = await _context.Employees
                .Include(e => e.User)
                .Include(e => e.Company)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
            {
                return NotFound("Employee profile not found");
            }

            // Get job count posted by this employee
            var jobs = await _context.Jobs
                .Include(j => j.JobStatus)
                .Where(j => j.PostedByEmployeeId == employee.Id)
                .ToListAsync();

            var activeJobs = jobs.Count(j => j.JobStatus.StatusName == "Active");

            // Get applications count for jobs posted by this employee
            var applications = await _context.Applications
                .Include(a => a.Status)
                .Where(a => jobs.Select(j => j.Id).Contains(a.JobId))
                .ToListAsync();

            var newApplications = applications.Count(a => a.Status.StatusName == "New" || a.Status.StatusName == "Pending");

            // Get recent job postings
            var recentJobs = await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.Location)
                .Include(j => j.EmploymentType)
                .Include(j => j.JobStatus)
                .Include(j => j.Applications)
                .Where(j => j.PostedByEmployeeId == employee.Id)
                .OrderByDescending(j => j.PostedDate)
                .Take(5)
                .ToListAsync();

            // Create dashboard view model
            var dashboardVM = new EmployeeDashboardViewModel
            {
                Employee = employee,
                ActiveJobsCount = activeJobs,
                TotalApplicationsCount = applications.Count,
                NewApplicationsCount = newApplications,
                RecentJobs = recentJobs
            };

            return View(dashboardVM);
        }
        
        // POST: Employee/CreateCompany
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany(CompanyCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if industry exists
                    var industry = await _industryRepository.GetAllAsync();
                    var existingIndustry = industry.FirstOrDefault(i => 
                        i.IndustryType.Equals(model.IndustryName, StringComparison.OrdinalIgnoreCase));
                    
                    int industryId;
                    
                    if (existingIndustry == null)
                    {
                        // Create new industry
                        var newIndustry = new Industry
                        {
                            IndustryType = model.IndustryName,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        
                        await _industryRepository.AddAsync(newIndustry);
                        industryId = newIndustry.Id;
                    }
                    else
                    {
                        industryId = existingIndustry.Id;
                    }
                    
                    // Create new company
                    var company = new Company
                    {
                        Name = model.Name,
                        FoundationDate = model.FoundationDate,
                        IndustryId = industryId,
                        Website = model.Website,
                        CompanySize = model.CompanySize,
                        HeadquartersLocation = model.HeadquartersLocation,
                        Description = model.Description,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    
                    // Handle logo upload if provided
                    if (model.LogoFile != null && model.LogoFile.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{model.LogoFile.FileName}";
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "logos");
                        
                        // Ensure directory exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.LogoFile.CopyToAsync(fileStream);
                        }
                        
                        company.LogoUrl = $"/uploads/logos/{uniqueFileName}";
                    }
                    
                    // Save company to get its ID
                    await _companyRepository.AddAsync(company);
                    
                    // Create branches
                    foreach (var branchVM in model.Branches)
                    {
                        if (!string.IsNullOrWhiteSpace(branchVM.BranchLocation))
                        {
                            var branch = new Branch
                            {
                                CompanyId = company.Id,
                                BranchLocation = branchVM.BranchLocation,
                                PhoneNumber = branchVM.PhoneNumber,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };
                            
                            await _branchRepository.AddAsync(branch);
                        }
                    }
                    
                    // Set TempData to pass the new company ID back to the form
                    TempData["NewCompanyId"] = company.Id;
                    TempData["SuccessMessage"] = $"Company '{company.Name}' created successfully!";
                    
                    // Return success JSON result with the new company ID and name
                    return Json(new { success = true, companyId = company.Id, companyName = company.Name });
                }
                catch (Exception ex)
                {
                    // Return error JSON
                    return Json(new { success = false, message = $"Error creating company: {ex.Message}" });
                }
            }
            
            // If validation failed, return the partial view
            return PartialView("_CompanyCreatePartial", model);
        }
    }
}
