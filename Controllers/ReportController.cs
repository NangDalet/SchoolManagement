using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models.Connection;

namespace SchoolManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowReport()
        {
            return View();
        }
        public IActionResult ViewReports(DateTime fromDate, DateTime toDate)
        {
            var res = from s in _context.Deposits
                      join c in _context.LevelDeposits on s.ID equals c.ClsDepositId
                      join u in _context.Users on s.ID equals u.Id
                      join l in _context.Levels on c.LevelCode equals l.LevelCode
                      where (s.PostingDate >= fromDate && s.PostingDate <= toDate)
                      select new
                      { s.InvoiceNo, s.StuName, s.PostingDate, s.Total, s.Deposit, s.BalanceDue, s.UserCode,l.LevelCode,l.LevelName };
            return Ok(res);
        }
    }
}
