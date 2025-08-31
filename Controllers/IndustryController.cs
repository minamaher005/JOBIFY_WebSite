using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace WebApplication2.Controllers
{
    public class IndustryController : Controller
    {
        private readonly IIndustryRepository _industryRepository;

        public IndustryController(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [HttpGet]
        public async Task<JsonResult> GetAllIndustries()
        {
            var industries = await _industryRepository.GetAllAsync();
            
            var selectListItems = industries.Select(i => new
            {
                id = i.Id,
                name = i.IndustryType
            }).ToList();
            
            return Json(selectListItems);
        }
    }
}
