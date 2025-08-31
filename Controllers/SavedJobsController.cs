using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    public class SavedJobsController : Controller
    {
        private readonly ISavedJobsService _savedJobsService;
        private readonly JobApplicationSystemContext _context;

        public SavedJobsController(
            ISavedJobsService savedJobsService,
            JobApplicationSystemContext context)
        {
            _savedJobsService = savedJobsService;
            _context = context;
        }

        // GET: SavedJobs
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated properly");
            }

            // Get job seeker ID from the user ID
            var jobSeeker = await _context.JobSeekers
                .FirstOrDefaultAsync(js => js.UserId == userId);

            if (jobSeeker == null)
            {
                return NotFound("Job seeker profile not found");
            }

            var savedJobs = await _savedJobsService.GetSavedJobsByJobSeekerId(jobSeeker.Id);
            return View(savedJobs);
        }

        // POST: SavedJobs/SaveJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                {
                    return NotFound("Job seeker profile not found");
                }

                await _savedJobsService.AddSavedJob(jobSeeker.Id, id);
                
                // Redirect back to the job details page or saved jobs list
                if (Request.Headers["Referer"].ToString().Contains("Jobs/Details"))
                {
                    TempData["SuccessMessage"] = "Job saved successfully!";
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                // Redirect back to the referring page
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                // Redirect back to the referring page
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        // POST: SavedJobs/UnsaveJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    .FirstOrDefaultAsync(js => js.UserId == userId);

                if (jobSeeker == null)
                {
                    return NotFound("Job seeker profile not found");
                }

                await _savedJobsService.DeleteSavedJob(jobSeeker.Id, id);
                
                TempData["SuccessMessage"] = "Job removed from saved jobs.";
                
                // If called from the saved jobs list, stay there, otherwise go back
                if (Request.Headers["Referer"].ToString().Contains("SavedJobs"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(Request.Headers["Referer"].ToString());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
