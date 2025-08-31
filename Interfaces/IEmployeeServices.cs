using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModels;
namespace WebApplication2.Interfaces
{
    public interface IEmployeeServices
    {
        Task CreateEmployeeAsync(EmployeeCreateViewModel employeeVM);
        Task EditEmployeeAsync(int id, EmployeeEditViewModel employeeVM);
       
    }
}