using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobCreateService _jobService;
        private readonly IJobRepository _jobRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmploymentTypeRepository _employmentTypeRepository;
        private readonly IJobLocationRepository _locationRepository;
        private readonly IJobStatusRepository _jobStatusRepository;
        private readonly ISavedJobsService _savedJobsService;
        private readonly JobApplicationSystemContext _context;

        public JobsController(
            IJobCreateService jobService,
            IJobRepository jobRepository,
            IEmployeeRepository employeeRepository,
            IEmploymentTypeRepository employmentTypeRepository,
            IJobLocationRepository locationRepository,
            IJobStatusRepository jobStatusRepository,
            ISavedJobsService savedJobsService,
            JobApplicationSystemContext context)
        {
            _jobService = jobService;
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;
            _employmentTypeRepository = employmentTypeRepository;
            _locationRepository = locationRepository;
            _jobStatusRepository = jobStatusRepository;
            _savedJobsService = savedJobsService;
            _context = context;
        }

        // GET: Jobs
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }
            
            // Get employee ID from the user ID
            var employeeId = await GetEmployeeIdByUserIdAsync(userId);
            
            if (employeeId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Get jobs posted by this employee
            var jobs = await _jobRepository.GetByEmployeeId(employeeId.Value);
            return View(jobs);
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            // Check if the user is an employee and if they own this job
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Employee"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var employeeId = await GetEmployeeIdByUserIdAsync(userId);
                
                ViewBag.IsJobOwner = (employeeId != null && job.PostedByEmployeeId == employeeId);
            }
            else
            {
                ViewBag.IsJobOwner = false;
            }

            // If user is logged in as a job seeker, check if they've saved this job
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("JobSeeker"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var jobSeeker = await _context.JobSeekers
                    .FirstOrDefaultAsync(js => js.UserId == userId);
                
                if (jobSeeker != null)
                {
                    // Check if this job is saved by the job seeker
                    var isSaved = await _context.SavedJobs
                        .AnyAsync(sj => sj.JobId == id && sj.JobSeekerId == jobSeeker.Id);
                    // viewbag here pass data to view directly without using ViewData or view model
                    ViewBag.IsSaved = isSaved;
                    ViewBag.JobSeekerId = jobSeeker.Id;
                }
            }

            return View(job);
        }

        // GET: Jobs/Create
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create(JobCreateVM jobVM, string? LocationText, string? EmploymentTypeText)
        {
            // Remove LocationId and EmploymentTypeId from ModelState validation
            // since we're using text inputs now
            ModelState.Remove("LocationId");
            ModelState.Remove("EmploymentTypeId");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Unauthorized("User is not authenticated properly");
                    }
                    
                    // Handle custom location
                    if (!string.IsNullOrEmpty(LocationText))
                    {
                        // Create new location or find existing one
                        var location = await _locationRepository.GetByNameAsync(LocationText);
                        
                        if (location == null)
                        {
                            // Create new location
                            location = new JobLocation { LocationName = LocationText };
                            await _context.JobLocations.AddAsync(location);
                            await _context.SaveChangesAsync();
                        }
                        
                        jobVM.LocationId = location.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("LocationText", "Location is required");
                        await PopulateDropdownsAsync();
                        return View(jobVM);
                    }
                    
                    // Handle custom employment type
                    if (!string.IsNullOrEmpty(EmploymentTypeText))
                    {
                        // Create new employment type or find existing one
                        var employmentType = await _employmentTypeRepository.GetByNameAsync(EmploymentTypeText);
                        
                        if (employmentType == null)
                        {
                            // Create new employment type
                            employmentType = new EmploymentType { TypeName = EmploymentTypeText };
                            await _context.EmploymentTypes.AddAsync(employmentType);
                            await _context.SaveChangesAsync();
                        }
                        
                        jobVM.EmploymentTypeId = employmentType.Id;
                    }
                    else
                    {
                        ModelState.AddModelError("EmploymentTypeText", "Employment Type is required");
                        await PopulateDropdownsAsync();
                        return View(jobVM);
                    }
                    
                    var job = await _jobService.CreateJobAsync(userId, jobVM);
                    TempData["SuccessMessage"] = "Job created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    // Log the error
                    Console.WriteLine($"Key not found error: {ex.Message}");
                }
                catch (DbUpdateException ex)
                {
                    // Handle database related errors (like foreign key violations)
                    ModelState.AddModelError("", $"Database error while creating job: {ex.InnerException?.Message ?? ex.Message}");
                    // Log the error
                    Console.WriteLine($"DB Error creating job: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating job: {ex.Message}");
                    // Log the error
                    Console.WriteLine($"Error creating job: {ex.Message}");
                }
            }

            await PopulateDropdownsAsync();
            return View(jobVM);
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            // Get the current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }
            
            // Get the employee ID
            var employeeId = await GetEmployeeIdByUserIdAsync(userId);
            
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            
            // Check if the current employee is the creator of the job
            if (employeeId == null || job.PostedByEmployeeId != employeeId)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this job.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            // Create JobEditVM from Job
            var jobVM = new JobEditVM
            {
                Id = job.Id,
                JobTitle = job.JobTitle,
                LocationId = job.LocationId,
                EmploymentTypeId = job.EmploymentTypeId,
                RequiredExperienceYears = job.RequiredExperienceYears,
                SalaryFrom = job.SalaryFrom,
                SalaryTo = job.SalaryTo,
                Currency = job.Currency,
                ApplicationDeadline = job.ApplicationDeadline,
                Requirements = job.Requirements,
                Benefits = job.Benefits,
                JobStatusId = job.JobStatusId
            };

            await PopulateDropdownsAsync();
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id, JobEditVM jobVM)
        {
            if (id != jobVM.Id)
            {
                return NotFound();
            }
            
            // Get the current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }
            
            // Get the employee ID
            var employeeId = await GetEmployeeIdByUserIdAsync(userId);
            
            // Check job ownership
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            
            // Check if the current employee is the creator of the job
            if (employeeId == null || job.PostedByEmployeeId != employeeId)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this job.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jobService.EditJobAsync(id, jobVM);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (!await JobExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Error updating job: {ex.Message}");
                    }
                }
            }

            await PopulateDropdownsAsync();
            return View(jobVM);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            // Get the current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }
            
            // Get the employee ID
            var employeeId = await GetEmployeeIdByUserIdAsync(userId);
            
            var job = await _jobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            
            // Check if the current employee is the creator of the job
            if (employeeId == null || job.PostedByEmployeeId != employeeId)
            {
                TempData["ErrorMessage"] = "You are not authorized to delete this job.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Get the current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User is not authenticated properly");
                }
                
                // Get the employee ID
                var employeeId = await GetEmployeeIdByUserIdAsync(userId);
                
                var job = await _jobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    return NotFound();
                }
                
                // Check if the current employee is the creator of the job
                if (employeeId == null || job.PostedByEmployeeId != employeeId)
                {
                    TempData["ErrorMessage"] = "You are not authorized to delete this job.";
                    return RedirectToAction(nameof(Details), new { id = id });
                }
                
                await _jobService.deleteJobAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting job: {ex.Message}");
                var job = await _jobRepository.GetByIdAsync(id);
                return View(job);
            }
        }

        private async Task<int?> GetEmployeeIdByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            return employee?.Id;
        }

        private async Task<bool> JobExists(int id)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                return job != null;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        private async Task PopulateDropdownsAsync()
        {
            // Populate dropdowns for create/edit views id here USED TO DEFINE WHICH DATA TO SHOW IN DROPDOWN AND TYPENAME IS THE 
            // DATA TO SHOW IN DROPDOWN
            ViewBag.EmploymentTypes = new SelectList(await _employmentTypeRepository.GetAllAsync(), "Id", "TypeName");
            ViewBag.Locations = new SelectList(await _locationRepository.GetAllAsync(), "Id", "LocationName");
            ViewBag.JobStatuses = new SelectList(await _jobStatusRepository.GetAllAsync(), "Id", "StatusName");
        }

        // GET: Jobs/SavedJobs
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> SavedJobs()
        {
            // CLAIMTYPES.NAMEIDENTIFIER is used to get the user ID of the currently logged-in user ASPBNETUSERS

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }

            // Get job seeker ID from the user ID
            var jobSeeker = await _context.JobSeekers
                .Include(js => js.User)
                .FirstOrDefaultAsync(js => js.UserId == userId);

            if (jobSeeker == null)
            {
                return NotFound("Job seeker profile not found");
            }

            var savedJobs = await _savedJobsService.GetSavedJobsByJobSeekerId(jobSeeker.Id);
            return View(savedJobs);
        }

        // POST: Jobs/SaveJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> SaveJob(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User is not authenticated properly");
                }

                // Get job seeker ID from the user ID
                var jobSeeker = await _context.JobSeekers
                    .Include(js => js.User)
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                {
                    return NotFound("Job seeker profile not found");
                }

                await _savedJobsService.AddSavedJob(jobSeeker.Id, id);
                
                TempData["SuccessMessage"] = "Job saved successfully!";
                return RedirectToAction("Details", new { id = id });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Details", new { id = id });
            }
        }

        // POST: Jobs/UnsaveJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UnsaveJob(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User is not authenticated properly");
                }

                // Get job seeker ID from the user ID
                var jobSeeker = await _context.JobSeekers
                    .Include(js => js.User)
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                {
                    return NotFound("Job seeker profile not found");
                }

                await _savedJobsService.DeleteSavedJob(jobSeeker.Id, id);
                
                // Check if the request is coming from the saved jobs page
                if (Request.Headers["Referer"].ToString().Contains("SavedJobs"))
                {
                    TempData["SuccessMessage"] = "Job removed from saved jobs.";
                    return RedirectToAction(nameof(SavedJobs));
                }
                else
                {
                    TempData["SuccessMessage"] = "Job removed from saved jobs.";
                    return RedirectToAction("Details", new { id = id });
                }
            }
            catch (Exception ex)
            {// THIS TEMP APPEAR IN THE CONTROLLER IN THE URL SECTION IN THE BROWSER
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Details", new { id = id });
            }
        }
        
        // GET: Jobs/AllJobs
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllJobs(string searchTerm, int? locationId, int? employmentTypeId)
        {
            var jobs = await _jobRepository.GetAllAsync();
            
            // Filter by status (only show active jobs)
            jobs = jobs.Where(j => j.JobStatus.StatusName == "Active").ToList();
            
            // Apply search filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                jobs = jobs.Where(j => 
                    j.JobTitle.ToLower().Contains(searchTerm) ||
                    (j.Requirements != null && j.Requirements.ToLower().Contains(searchTerm)) ||
                    j.Company.Name.ToLower().Contains(searchTerm)
                ).ToList();
            }
            
            if (locationId.HasValue)
            {
                jobs = jobs.Where(j => j.LocationId == locationId.Value).ToList();
            }
            
            if (employmentTypeId.HasValue)
            {
                jobs = jobs.Where(j => j.EmploymentTypeId == employmentTypeId.Value).ToList();
            }
            
            // Get the current employee ID for ownership checks in the view
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentEmployeeId = await GetEmployeeIdByUserIdAsync(userId);
            ViewBag.CurrentEmployeeId = currentEmployeeId;
            
            // Populate dropdown data
            await PopulateDropdownsAsync();
            
            // Create view model
            var viewModel = new JobSearchViewModel
            {
                Jobs = jobs,
                SearchTerm = searchTerm,
                LocationId = locationId,
                EmploymentTypeId = employmentTypeId
            };
            
            return View(viewModel);
        }

        // GET: Jobs/Search
        public async Task<IActionResult> Search(string searchTerm, int? locationId, int? employmentTypeId)
        {
            var jobs = await _jobRepository.GetAllAsync();
            
            // Filter by status (only show active jobs)
            jobs = jobs.Where(j => j.JobStatus.StatusName == "Active").ToList();
            
            // Apply search filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // THIS IS THE SEARCH FUNCTION TO FIND JOBS
                searchTerm = searchTerm.ToLower();
                jobs = jobs.Where(j => 
                    j.JobTitle.ToLower().Contains(searchTerm) ||
                    (j.Requirements != null && j.Requirements.ToLower().Contains(searchTerm)) ||
                    j.Company.Name.ToLower().Contains(searchTerm)
                ).ToList();
            }
            
            if (locationId.HasValue)
            {
                jobs = jobs.Where(j => j.LocationId == locationId.Value).ToList();
            }
            
            if (employmentTypeId.HasValue)
            {
                jobs = jobs.Where(j => j.EmploymentTypeId == employmentTypeId.Value).ToList();
            }
            
            // Populate dropdown data
            await PopulateDropdownsAsync();
            
            // Create view model
            var viewModel = new JobSearchViewModel
            {
                Jobs = jobs,
                SearchTerm = searchTerm,
                LocationId = locationId,
                EmploymentTypeId = employmentTypeId
            };
            
            return View(viewModel);
        }
    }
}
