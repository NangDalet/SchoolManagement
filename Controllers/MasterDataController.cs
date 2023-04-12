using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using System.Data;

namespace SchoolManagement.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MasterDataController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult getLevel()
        {
            List<SelectListItem> levels = _context.Levels.AsNoTracking()
              .OrderBy(n => n.LevelName)
                       .Select(n =>
                       new SelectListItem
                       {
                           Value = n.Id.ToString(),
                           Text = n.LevelName
                       }).ToList();
            ViewBag.Levels = levels;
            return Ok(levels);
        }
        public IActionResult SelectLevel()
        {
            var Level = from r in _context.Levels
                        select new
                        { r.LevelCode, r.LevelName, r.UnitPrice, };
            return Ok(Level);
        }
    }
}
