using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApplication2.Models;
using WebApplication2.ViewModels;
namespace WebApplication2.Interfaces
{
    public interface IJobCreateService
    {
        public Task<Job> CreateJobAsync(string userId, JobCreateVM jobVM);
        public Task<Job> EditJobAsync(int id, JobEditVM jobVM);
        public Task<Job> deleteJobAsync(int id);
        
    }
}