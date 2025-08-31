using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<JobApplicationSystemContext>();
            
            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();
            
            await SeedIdentityRoles(roleManager);
            await SeedIndustries(context);
            await SeedCompanies(context);
            await SeedEmployeeRoles(context);
            await SeedEducationLevels(context);
            await SeedSkills(context);
            await SeedApplicationStatuses(context);
        }
        
        private static async Task SeedApplicationStatuses(JobApplicationSystemContext context)
        {
            if (!context.ApplicationStatuses.Any())
            {
                var statuses = new List<ApplicationStatus>
                {
                    new ApplicationStatus 
                    { 
                        Id = 1,
                        StatusName = "Pending", 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    },
                    new ApplicationStatus 
                    { 
                        Id = 2,
                        StatusName = "Reviewed", 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    },
                    new ApplicationStatus 
                    { 
                        Id = 3,
                        StatusName = "Interview", 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    },
                    new ApplicationStatus 
                    { 
                        Id = 4,
                        StatusName = "Accepted", 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    },
                    new ApplicationStatus 
                    { 
                        Id = 5,
                        StatusName = "Rejected", 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    }
                };
                
                await context.ApplicationStatuses.AddRangeAsync(statuses);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedIdentityRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Employee", "JobSeeker" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        
        private static async Task SeedCompanies(JobApplicationSystemContext context)
        {
            if (!context.Companies.Any())
            {
                var companies = new List<Company>
                {
                    new Company
                    {
                        Name = "Microsoft",
                        Description = "Global technology company that develops, manufactures, licenses, supports, and sells computer software, consumer electronics, personal computers, and related services.",
                        Website = "https://www.microsoft.com",
                        LogoUrl = "/images/logos/microsoft.png",
                        HeadquartersLocation = "One Microsoft Way, Redmond, WA",
                        IndustryId = 1, // Technology
                        FoundationDate = new DateOnly(1975, 4, 4),
                        CompanySize = 181000,
                        Rating = 4.2m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Company
                    {
                        Name = "Google",
                        Description = "Multinational technology company that specializes in Internet-related services and products.",
                        Website = "https://www.google.com",
                        LogoUrl = "/images/logos/google.png",
                        HeadquartersLocation = "1600 Amphitheatre Parkway, Mountain View, CA",
                        IndustryId = 1, // Technology
                        FoundationDate = new DateOnly(1998, 9, 4),
                        CompanySize = 156000,
                        Rating = 4.5m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Company
                    {
                        Name = "Amazon",
                        Description = "Multinational technology company focusing on e-commerce, cloud computing, digital streaming, and artificial intelligence.",
                        Website = "https://www.amazon.com",
                        LogoUrl = "/images/logos/amazon.png",
                        HeadquartersLocation = "410 Terry Ave N, Seattle, WA",
                        IndustryId = 1, // Technology
                        FoundationDate = new DateOnly(1994, 7, 5),
                        CompanySize = 1300000,
                        Rating = 4.0m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Company
                    {
                        Name = "Meta",
                        Description = "American social media conglomerate corporation formerly known as Facebook.",
                        Website = "https://www.meta.com",
                        LogoUrl = "/images/logos/meta.png",
                        HeadquartersLocation = "1 Hacker Way, Menlo Park, CA",
                        IndustryId = 1, // Technology
                        FoundationDate = new DateOnly(2004, 2, 4),
                        CompanySize = 77000,
                        Rating = 4.1m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Company
                    {
                        Name = "Apple",
                        Description = "American multinational technology company that specializes in consumer electronics, computer software, and online services.",
                        Website = "https://www.apple.com",
                        LogoUrl = "/images/logos/apple.png",
                        HeadquartersLocation = "1 Apple Park Way, Cupertino, CA",
                        IndustryId = 1, // Technology
                        FoundationDate = new DateOnly(1976, 4, 1),
                        CompanySize = 154000,
                        Rating = 4.4m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };
                
                await context.Companies.AddRangeAsync(companies);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedEmployeeRoles(JobApplicationSystemContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        RoleName = "HR Manager",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Role
                    {
                        RoleName = "Recruiter",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Role
                    {
                        RoleName = "Department Manager",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Role
                    {
                        RoleName = "CEO",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Role
                    {
                        RoleName = "CTO",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Role
                    {
                        RoleName = "Team Lead",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };
                
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedIndustries(JobApplicationSystemContext context)
        {
            if (!context.Industries.Any())
            {
                var industries = new List<Industry>
                {
                    new Industry { IndustryType = "Technology", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Healthcare", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Finance", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Education", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Manufacturing", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Retail", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Hospitality", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Industry { IndustryType = "Entertainment", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                
                await context.Industries.AddRangeAsync(industries);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedEducationLevels(JobApplicationSystemContext context)
        {
            if (!context.EducationLevels.Any())
            {
                var educationLevels = new List<EducationLevel>
                {
                    new EducationLevel { LevelName = "High School", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new EducationLevel { LevelName = "Associate's Degree", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new EducationLevel { LevelName = "Bachelor's Degree", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new EducationLevel { LevelName = "Master's Degree", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new EducationLevel { LevelName = "Doctorate", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new EducationLevel { LevelName = "Professional Certificate", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                
                await context.EducationLevels.AddRangeAsync(educationLevels);
                await context.SaveChangesAsync();
            }
        }
        
        private static async Task SeedSkills(JobApplicationSystemContext context)
        {
            if (!context.Skills.Any())
            {
                var skills = new List<Skill>
                {
                    // Programming Languages
                    new Skill { SkillName = "C#", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Java", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "JavaScript", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Python", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "TypeScript", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "PHP", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Swift", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Kotlin", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    
                    // Frameworks
                    new Skill { SkillName = "ASP.NET Core", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "React", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Angular", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Vue.js", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Django", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Laravel", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Spring Boot", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Flutter", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    
                    // Databases
                    new Skill { SkillName = "SQL Server", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "MySQL", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "PostgreSQL", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "MongoDB", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Oracle", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    
                    // Cloud Platforms
                    new Skill { SkillName = "AWS", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Azure", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Google Cloud", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    
                    // Soft Skills
                    new Skill { SkillName = "Project Management", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Team Leadership", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Communication", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Skill { SkillName = "Problem Solving", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                
                await context.Skills.AddRangeAsync(skills);
                await context.SaveChangesAsync();
            }
        }
    }
}
