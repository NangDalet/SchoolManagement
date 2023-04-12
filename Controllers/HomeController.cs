using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using System.Diagnostics;

namespace SchoolManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Count User
            ViewBag.totalUser = _context.Users.Where(u => u.Status == "Active").Count();
            //Count Level
            ViewBag.totalLevel = _context.Levels.Where(u => u.Status == "Active").Count();
            //Sum Total Price
            var sumTotal = (from t in _context.Deposits select t.Total).Sum();
            ViewBag.toTalSalary = sumTotal;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Code")))
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        public IActionResult GetStatistic()
        {
            var statistic = from l in _context.LevelDeposits
                            join d in _context.Deposits on l.ClsDepositId equals d.ID
                            join v in _context.Levels on l.LevelCode equals v.LevelCode
                            group v by new { v.LevelCode, v.LevelName } into g
                            select new
                            {
                                Name = g.Key.LevelName,
                                Number = g.Count()
                            };
            return Ok(statistic);
        }
        public IActionResult TotalPrice()
        {
            var sumTotal = (from t in _context.Deposits select t.Total).Sum();
            ViewBag.toTalSalary = sumTotal;
            return Ok(sumTotal);
        }
    }
}