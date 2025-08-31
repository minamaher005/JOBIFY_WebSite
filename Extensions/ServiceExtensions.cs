using WebApplication2.Interfaces;
using WebApplication2.Services.JobSeekerService;
using WebApplication2.Repository;
using WebApplication2.Services.FileUploadService;
using WebApplication2.Services.Job_service;
using WebApplication2.Services;
using WebApplication2.Services.Application_service;
using WebApplication2.Models;

namespace WebApplication2.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IJobSeekerService, JobSeekerService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IEmployeeServices, Services.EmployeeService.EmployeeCreateService>();
            services.AddScoped<IJobCreateService, JobService>();
            services.AddScoped<ISavedJobsService, JobSaveService>();
            services.AddScoped<IJobApplyService, JobApplyService>();
            
            // Register repositories
            services.AddScoped<IJobSeeker, JobSeekerRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICompanyREpository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IEducationLevelRepository, EducationLevelRepository>();
            services.AddScoped<IJobSeekerSkillRepository, JobSeekerSkillRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobStatusRepository, JobStatusRepository>();
            services.AddScoped<IJobLocationRepository, JobLocationRepository>();
            services.AddScoped<IEmploymentTypeRepository, EmploymentTypeRepository>();
            services.AddScoped<ISavedJobRepository, SavedJobRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IIndustryRepository, IndustryRepository>();
            services.AddScoped<IApplicationStatusRepository>(provider =>
            {
                var context = provider.GetRequiredService<JobApplicationSystemContext>();
                return new ApplicationStatusRepository(context);
            });
            
            // Add other services here
            
            return services;
        }
    }
}
