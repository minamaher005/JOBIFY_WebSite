using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.Services.JobSeekerService
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly JobApplicationSystemContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileUploadService _fileUploadService;

        public JobSeekerService(
            JobApplicationSystemContext context,
            UserManager<ApplicationUser> userManager,
            IFileUploadService fileUploadService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
        }

        public async Task createjobSeeker(JobSeekerRegisterViewModel jobVM)
        {
            if (jobVM == null)
            {
                throw new ArgumentNullException(nameof(jobVM), "Job seeker view model cannot be null");
            }

            // Create the Identity user first
            var user = new ApplicationUser
            {
                UserName = jobVM.Email,
                Email = jobVM.Email,
                PhoneNumber = jobVM.PhoneNumber,
                FirstName = jobVM.FirstName,
                LastName = jobVM.LastName
            };

            // Create the user with password
            var result = await _userManager.CreateAsync(user, jobVM.Password);

            if (!result.Succeeded)
            {
                // Handle the error - throw an exception with the error details
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            // Assign the user to the JobSeeker role
            await _userManager.AddToRoleAsync(user, "JobSeeker");

            // Now create the JobSeeker profile linked to this user
            var jobSeeker = new JobSeeker
            {
                UserId = user.Id,
                FullName = $"{jobVM.FirstName} {jobVM.LastName}",
                DateOfBirth = jobVM.DateOfBirth,
                City = jobVM.City,
                Country = jobVM.Country,
                Bio = jobVM.Bio,
                YearsOfExperience = jobVM.YearsOfExperience,
                UniversityName = jobVM.UniversityName,
                EducationLevelId = jobVM.EducationLevelId,
                LinkedInUrl = jobVM.LinkedInUrl,
                PortfolioUrl = jobVM.PortfolioUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Handle profile picture upload
            if (jobVM.ProfilePicture != null && jobVM.ProfilePicture.Length > 0)
            {
                jobSeeker.ProfilePictureUrl = await _fileUploadService.UploadFileAsync(
                    jobVM.ProfilePicture, 
                    "profile-pictures", 
                    $"user_{user.Id}");
            }
            
            // Handle resume upload
            if (jobVM.Resume != null && jobVM.Resume.Length > 0)
            {
                jobSeeker.ResumeUrl = await _fileUploadService.UploadFileAsync(
                    jobVM.Resume, 
                    "resumes", 
                    $"user_{user.Id}");
            }

            _context.JobSeekers.Add(jobSeeker);
            await _context.SaveChangesAsync();
            
            // Handle skills
            // First add selected skills from dropdown
            if (jobVM.SelectedSkills != null && jobVM.SelectedSkills.Any())
            {
                foreach (var skillId in jobVM.SelectedSkills)
                {
                    _context.JobSeekerSkills.Add(new JobSeekerSkill
                    {
                        JobSeekerId = jobSeeker.Id,
                        SkillId = skillId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
            
            // Then handle manually entered skills
            if (!string.IsNullOrWhiteSpace(jobVM.ManualSkills))
            {
                // Split by comma and trim each skill
                var manualSkills = jobVM.ManualSkills.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
                
                foreach (var skillName in manualSkills)
                {
                    // Check if skill already exists in the database
                    var existingSkill = await _context.Skills
                        .FirstOrDefaultAsync(s => s.SkillName.ToLower() == skillName.ToLower());
                    
                    int skillId;
                    
                    if (existingSkill == null)
                    {
                        // Create new skill
                        var newSkill = new Skill
                        {
                            SkillName = skillName,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        _context.Skills.Add(newSkill);
                        await _context.SaveChangesAsync();
                        
                        skillId = newSkill.Id;
                    }
                    else
                    {
                        skillId = existingSkill.Id;
                    }
                    
                    // Add the skill to the job seeker
                    _context.JobSeekerSkills.Add(new JobSeekerSkill
                    {
                        JobSeekerId = jobSeeker.Id,
                        SkillId = skillId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
            
            await _context.SaveChangesAsync();
        }
        
        public async Task EditJobSeeker(int id, JobSeekerEditViewModel jobVM)
        {
            if (jobVM == null)
            {
                throw new ArgumentNullException(nameof(jobVM), "Job seeker view model cannot be null");
            }

            var jobSeeker = await _context.JobSeekers
                .Include(js => js.User)
                .Include(js => js.JobSeekerSkills)
                .FirstOrDefaultAsync(js => js.Id == id);

            if (jobSeeker == null)
            {
                throw new KeyNotFoundException($"Job seeker with ID {id} not found");
            }

            if (jobSeeker.User == null)
            {
                throw new KeyNotFoundException($"User not found for job seeker with ID {id}");
            }

            // Update the User properties
            jobSeeker.User.PhoneNumber = jobVM.PhoneNumber;
            jobSeeker.User.FirstName = jobVM.FirstName ?? string.Empty;
            jobSeeker.User.LastName = jobVM.LastName ?? string.Empty;

            // Update JobSeeker properties
            jobSeeker.FullName = $"{jobVM.FirstName} {jobVM.LastName}";
            jobSeeker.YearsOfExperience = jobVM.YearsOfExperience;
            jobSeeker.EducationLevelId = jobVM.EducationLevelId;
            jobSeeker.Bio = jobVM.Bio;
            jobSeeker.LinkedInUrl = jobVM.LinkedInUrl;
            jobSeeker.PortfolioUrl = jobVM.PortfolioUrl;
            
            // Handle profile picture upload
            if (jobVM.ProfilePicture != null && jobVM.ProfilePicture.Length > 0)
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(jobSeeker.ProfilePictureUrl))
                {
                    _fileUploadService.DeleteFile(jobSeeker.ProfilePictureUrl);
                }
                
                // Upload new file
                jobSeeker.ProfilePictureUrl = await _fileUploadService.UploadFileAsync(
                    jobVM.ProfilePicture, 
                    "profile-pictures", 
                    $"user_{jobSeeker.UserId}");
            }
            
            // Handle resume upload
            if (jobVM.Resume != null && jobVM.Resume.Length > 0)
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(jobSeeker.ResumeUrl))
                {
                    _fileUploadService.DeleteFile(jobSeeker.ResumeUrl);
                }
                
                // Upload new file
                jobSeeker.ResumeUrl = await _fileUploadService.UploadFileAsync(
                    jobVM.Resume, 
                    "resumes", 
                    $"user_{jobSeeker.UserId}");
            }
            
            // Handle skills
            // Remove existing skills
            var existingSkills = jobSeeker.JobSeekerSkills.ToList();
            if (existingSkills.Any())
            {
                _context.JobSeekerSkills.RemoveRange(existingSkills);
            }
            
            // Add skills from dropdown
            if (jobVM.SelectedSkills != null && jobVM.SelectedSkills.Any())
            {
                foreach (var skillId in jobVM.SelectedSkills)
                {
                    _context.JobSeekerSkills.Add(new JobSeekerSkill
                    {
                        JobSeekerId = jobSeeker.Id,
                        SkillId = skillId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
            
            // Handle manually entered skills
            if (!string.IsNullOrWhiteSpace(jobVM.ManualSkills))
            {
                // Split by comma and trim each skill
                var manualSkills = jobVM.ManualSkills.Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();
                
                foreach (var skillName in manualSkills)
                {
                    // Check if skill already exists in the database
                    var existingSkill = await _context.Skills
                        .FirstOrDefaultAsync(s => s.SkillName.ToLower() == skillName.ToLower());
                    
                    int skillId;
                    
                    if (existingSkill == null)
                    {
                        // Create new skill
                        var newSkill = new Skill
                        {
                            SkillName = skillName,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        
                        _context.Skills.Add(newSkill);
                        await _context.SaveChangesAsync();
                        
                        skillId = newSkill.Id;
                    }
                    else
                    {
                        skillId = existingSkill.Id;
                    }
                    
                    // Add the skill to the job seeker
                    _context.JobSeekerSkills.Add(new JobSeekerSkill
                    {
                        JobSeekerId = jobSeeker.Id,
                        SkillId = skillId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
            }
            
            jobSeeker.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
