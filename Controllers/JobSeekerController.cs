using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly JobApplicationSystemContext _context;
        private readonly IJobSeeker _jobSeeker;
        private readonly IJobSeekerService _jobSeekerService;
        private readonly ISavedJobsService _savedJobsService;
        private readonly IJobRepository _jobRepository;

        public JobSeekerController(
            JobApplicationSystemContext context,
            IJobSeeker jobSeeker,
            IJobSeekerService jobSeekerService,
            ISavedJobsService savedJobsService,
            IJobRepository jobRepository)
        {
            _context = context;
            _jobSeeker = jobSeeker;
            _jobSeekerService = jobSeekerService;
            _savedJobsService = savedJobsService;
            _jobRepository = jobRepository;
        }

        // GET: JobSeeker/Create
        public async Task<IActionResult> Create()
        {
            // Load education levels for the dropdown
            ViewBag.EducationLevels = await _context.EducationLevels.ToListAsync();
            
            // Load skills for multi-select
            var allSkills = await _context.Skills.ToListAsync();
            var model = new JobSeekerRegisterViewModel
            {
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                AvailableSkills = allSkills.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SkillName
                }).ToList()
            };
            
            return View(model);
        }

        // POST: JobSeeker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobSeekerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _jobSeekerService.createjobSeeker(model);
                    return RedirectToAction("Index", "Home", new { message = "Registration successful! Please log in." });
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
            // Reload education levels for the dropdown
            ViewBag.EducationLevels = await _context.EducationLevels.ToListAsync();
            
            // Reload skills for multi-select
            var allSkills = await _context.Skills.ToListAsync();
            model.AvailableSkills = allSkills.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SkillName,
                Selected = model.SelectedSkills != null && model.SelectedSkills.Contains(s.Id)
            }).ToList();
            
            return View(model);
        }
        
        public async Task<IActionResult> edit(int id)
        {
            var jobseeker = await _context.JobSeekers
                .Include(js => js.User)
                .Include(js => js.JobSeekerSkills)
                .FirstOrDefaultAsync(js => js.Id == id);
                
            if (jobseeker == null) throw new KeyNotFoundException($"Job seeker with ID {id} not found");
            if (jobseeker.User == null) throw new KeyNotFoundException($"User not found for job seeker with ID {id}");
            
            // Load education levels for the dropdown
            ViewBag.EducationLevels = await _context.EducationLevels.ToListAsync();
            
            // Load skills for multi-select
            var allSkills = await _context.Skills.ToListAsync();
            var jobSeekerSkillIds = jobseeker.JobSeekerSkills
                .Select(jss => jss.SkillId)
                .ToList();
            
            var jobVM = new JobSeekerEditViewModel
            {
                PhoneNumber = jobseeker.User.PhoneNumber,
                FirstName = jobseeker.User.FirstName ?? string.Empty,
                LastName = jobseeker.User.LastName ?? string.Empty,
                YearsOfExperience = jobseeker.YearsOfExperience,
                Bio = jobseeker.Bio,
                LinkedInUrl = jobseeker.LinkedInUrl,
                EducationLevelId = jobseeker.EducationLevelId,
                PortfolioUrl = jobseeker.PortfolioUrl,
                CurrentProfilePictureUrl = jobseeker.ProfilePictureUrl,
                CurrentResumeUrl = jobseeker.ResumeUrl,
                SelectedSkills = jobSeekerSkillIds,
                AvailableSkills = allSkills.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SkillName,
                    Selected = jobSeekerSkillIds.Contains(s.Id)
                }).ToList()
            };
            
            return View(jobVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int id, JobSeekerEditViewModel jobVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _jobSeekerService.EditJobSeeker(id, jobVM);
                    return RedirectToAction("Dashboard", "JobSeeker", new { message = "Profile updated successfully!" });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while updating the profile: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            // Reload education levels for the dropdown
            ViewBag.EducationLevels = await _context.EducationLevels.ToListAsync();
            
            // Reload skills for multi-select
            var allSkills = await _context.Skills.ToListAsync();
            jobVM.AvailableSkills = allSkills.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SkillName,
                Selected = jobVM.SelectedSkills != null && jobVM.SelectedSkills.Contains(s.Id)
            }).ToList();
            
            return View(jobVM);
        }

        // GET: JobSeeker/Dashboard
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }

            // Get job seeker from the user ID
            var jobSeeker = await _context.JobSeekers
                .Include(js => js.User)
                .Include(js => js.JobSeekerSkills)
                .ThenInclude(jss => jss.Skill)
                .Include(js => js.EducationLevel)
                .FirstOrDefaultAsync(js => js.UserId == userId);

            if (jobSeeker == null)
            {
                return NotFound("Job seeker profile not found");
            }

            // Get saved jobs count
            var savedJobs = await _context.SavedJobs
                .Where(sj => sj.JobSeekerId == jobSeeker.Id)
                .ToListAsync();

            // Get applied jobs count
            var appliedJobs = await _context.Applications
                .Where(a => a.JobSeekerId == jobSeeker.Id)
                .ToListAsync();

            // Get recent job postings
            var recentJobs = await _context.Jobs
                .Include(j => j.Company)
                .Include(j => j.Location)
                .Include(j => j.EmploymentType)
                .OrderByDescending(j => j.PostedDate)
                .Take(5)
                .ToListAsync();

            // Create dashboard view model
            var dashboardVM = new JobSeekerDashboardViewModel
            {
                JobSeeker = jobSeeker,
                SavedJobsCount = savedJobs.Count,
                AppliedJobsCount = appliedJobs.Count,
                RecentJobs = recentJobs,
                Skills = jobSeeker.JobSeekerSkills.Select(jss => jss.Skill).ToList()
            };

            return View(dashboardVM);
        }
    }
}