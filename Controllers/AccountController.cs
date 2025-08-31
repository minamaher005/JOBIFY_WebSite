using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.ViewModels.emp;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JobApplicationSystemContext _context;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyREpository _companyRepository;
        
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JobApplicationSystemContext context,
            IRoleRepository roleRepository,
            ICompanyREpository companyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult RegisterJobSeeker()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterJobSeeker(JobSeekerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the ApplicationUser with Identity information
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    // Assign the JobSeeker role
                    await _userManager.AddToRoleAsync(user, "JobSeeker");
                    
                    // Create the JobSeeker profile
                    var jobSeeker = new JobSeeker
                    {
                        UserId = user.Id,
                        FullName = $"{model.FirstName} {model.LastName}",
                        DateOfBirth = model.DateOfBirth,
                        City = model.City,
                        Country = model.Country,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    _context.JobSeekers.Add(jobSeeker);
                    await _context.SaveChangesAsync();
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> RegisterEmployee()
        {
            var roles = await _roleRepository.GetAllAsync();
            var companies = await _companyRepository.GetAllAsync();
            
            ViewBag.Roles = roles;
            ViewBag.Companies = companies;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterEmployee(EmployeeRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the ApplicationUser with Identity information
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    // Assign the Employee role
                    await _userManager.AddToRoleAsync(user, "Employee");
                    
                    // Create the Employee profile
                    var employee = new Employee
                    {
                        UserId = user.Id,
                        FullName = $"{model.FirstName} {model.LastName}",
                        RoleId = model.RoleId,
                        CompanyId = model.CompanyId,
                        PositionTitle = model.PositionTitle,
                        DateJoined = DateOnly.FromDateTime(DateTime.UtcNow),
                        IsActive = true,
                        City = model.City,
                        Country = model.Country,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            var roles = await _roleRepository.GetAllAsync();
            var companies = await _companyRepository.GetAllAsync();
            
            ViewBag.Roles = roles;
            ViewBag.Companies = companies;
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Get the current user
                        var user = await _userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            
                            // Redirect based on role
                            if (roles.Contains("JobSeeker"))
                            {
                                return RedirectToAction("Dashboard", "JobSeeker");
                            }
                            else if (roles.Contains("Employee"))
                            {
                                return RedirectToAction("Dashboard", "Employee");
                            }
                            else if (roles.Contains("Admin"))
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                        }
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
