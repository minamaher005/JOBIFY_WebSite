using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IJobApplyService _jobApplyService;
        private readonly IJobRepository _jobRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly JobApplicationSystemContext _context;

        public ApplicationsController(
            IJobApplyService jobApplyService,
            IJobRepository jobRepository,
            IApplicationStatusRepository applicationStatusRepository,
            JobApplicationSystemContext context)
        {
            _jobApplyService = jobApplyService;
            _jobRepository = jobRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _context = context;
        }

        // GET: Applications/Apply/5 (JobId)
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Apply(int id)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                
                // Check if the job seeker has already applied
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Challenge();
                }

                var hasApplied = await _jobApplyService.HasJobSeekerAppliedToJobAsync(id, userId);
                if (hasApplied)
                {
                    TempData["ErrorMessage"] = "You have already applied for this job.";
                    return RedirectToAction("Details", "Jobs", new { id = id });
                }

                var viewModel = new ApplicationCreateViewModel
                {
                    JobId = id
                };

                // Pass job details to the view for display
                ViewBag.Job = job;

                return View(viewModel);
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "The job you're trying to apply for could not be found.";
                return RedirectToAction("Search", "Jobs");
            }
            catch (Exception)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while loading the job. Please try again.";
                
                var viewModel = new ApplicationCreateViewModel
                {
                    JobId = id
                };
                
                return View(viewModel);
            }
        }

        // POST: Applications/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Apply(ApplicationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    // Try to fetch job details again for the view
                    var job = await _jobRepository.GetByIdAsync(model.JobId);
                    ViewBag.Job = job;
                }
                catch
                {
                    // If job can't be fetched, continue without it
                }
                
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Challenge();
                }

                // Check if resume file is uploaded
                if (model.Resume == null || model.Resume.Length == 0)
                {
                    ModelState.AddModelError("Resume", "Please upload your resume");
                    var job = await _jobRepository.GetByIdAsync(model.JobId);
                    ViewBag.Job = job;
                    return View(model);
                }

                // Validate file type (PDF, DOC, DOCX)
                var allowedFileTypes = new[] { ".pdf", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(model.Resume.FileName).ToLowerInvariant();
                if (!allowedFileTypes.Contains(fileExtension))
                {
                    ModelState.AddModelError("Resume", "Only PDF, DOC, and DOCX files are allowed");
                    var job = await _jobRepository.GetByIdAsync(model.JobId);
                    ViewBag.Job = job;
                    return View(model);
                }

                // Validate file size (max 5MB)
                if (model.Resume.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Resume", "File size cannot exceed 5MB");
                    var job = await _jobRepository.GetByIdAsync(model.JobId);
                    ViewBag.Job = job;
                    return View(model);
                }

                // Log the model values for debugging
                Console.WriteLine($"Application submission - JobId: {model.JobId}, Resume filename: {model.Resume.FileName}");

                var application = await _jobApplyService.CreateApplicationAsync(userId, model);
                
                TempData["SuccessMessage"] = "Your application has been submitted successfully!";
                return RedirectToAction("MyApplications");
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Application error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                ModelState.AddModelError("", "An error occurred while submitting your application: " + ex.Message);
                
                try
                {
                    // Try to fetch job details again for the view
                    var job = await _jobRepository.GetByIdAsync(model.JobId);
                    ViewBag.Job = job;
                }
                catch
                {
                    // If job can't be fetched, continue without it
                }
                
                return View(model);
            }
        }

        // GET: Applications/MyApplications
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> MyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            
            var applications = await _jobApplyService.GetApplicationsByJobSeekerAsync(userId);
            return View(applications);
        }

        // GET: Applications/JobApplications/5 (JobId)
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> JobApplications(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            // Check if the employee is authorized to view these applications
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applications = await _jobApplyService.GetApplicationsByJobIdAsync(id);
            
            ViewBag.JobTitle = job.JobTitle;
            ViewBag.JobId = job.Id;
            
            return View(applications);
        }

        // GET: Applications/AllApplications
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            
            var applications = await _jobApplyService.GetApplicationsForEmployeeAsync(userId);
            return View(applications);
        }

        // GET: Applications/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var application = await _jobApplyService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Check if the user is authorized to view this application
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            
            // If job seeker, check if it's their application
            if (User.IsInRole("JobSeeker") && application.JobSeeker.UserId != userId)
            {
                return Forbid();
            }
            
            // If employee, check if they posted the job
            if (User.IsInRole("Employee") && application.Job?.PostedByEmployee?.UserId != userId)
            {
                return Forbid();
            }

            return View(application);
        }

        // GET: Applications/UpdateStatus/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var application = await _jobApplyService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Check if the employee is authorized to update this application
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }
            
            if (application.Job?.PostedByEmployee?.UserId != userId)
            {
                return Forbid();
            }

            // Get all application statuses for dropdown
            ViewBag.Statuses = new SelectList(await _applicationStatusRepository.GetAllAsync(), "Id", "StatusName");
            
            return View(application);
        }

        // POST: Applications/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateStatus(int id, int statusId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId == null)
                    {
                        return Challenge();
                    }
                    
                    await _jobApplyService.UpdateApplicationStatusAsync(id, statusId, userId);
                    
                    TempData["SuccessMessage"] = "Application status updated successfully!";
                    return RedirectToAction("Details", new { id = id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            var application = await _jobApplyService.GetApplicationByIdAsync(id);
            ViewBag.Statuses = new SelectList(await _applicationStatusRepository.GetAllAsync(), "Id", "StatusName");
            
            return View(application);
        }
    }
}
