using WebApplication2.Models;
using WebApplication2.ViewModels;
using System.Threading.Tasks;

namespace WebApplication2.Interfaces
{
    public interface IJobSeekerService
    {
        Task createjobSeeker(JobSeekerRegisterViewModel jobVM);
        Task EditJobSeeker(int id, JobSeekerEditViewModel jobVM);
    }
}
