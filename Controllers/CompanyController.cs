using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyREpository _companyRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly JobApplicationSystemContext _context;

        public CompanyController(ICompanyREpository companyRepository, IBranchRepository branchRepository, JobApplicationSystemContext context)
        {
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            _context = context;
        }

        // GET: /Company/Index
        public async Task<IActionResult> Index()
        {
            // Get companies with their industries
            var companies = await _context.Companies
                .Include(c => c.Industry)
                .ToListAsync();
            return View(companies);
        }

        // GET: /Company/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Industry)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            // Get branches for this company
            var branches = await _branchRepository.GetByCompanyAsync(company.Id);
            ViewBag.Branches = branches;

            return View(company);
        }
    }
}
