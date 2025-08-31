using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using WebApplication2.Models;
namespace WebApplication2.Interfaces
{
    public interface ISavedJobsService
    {
        Task<IEnumerable<SavedJob>> AddSavedJob(int jobSeekerId, int jobId);
        Task<IEnumerable<SavedJob>> GetSavedJobsByJobSeekerId(int jobSeekerId);
        Task<SavedJob> DeleteSavedJob(int jobSeekerId, int jobId);
        
    }
}