using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Interfaces;
public interface IJobSeeker
{
      Task<JobSeeker> GetByIdAsync(int id);
    Task<IEnumerable<JobSeeker>> GetAllAsync();
    Task AddAsync(JobSeeker jobSeeker);
    Task UpdateAsync(JobSeeker jobSeeker);
    Task DeleteAsync(int id);


    

}