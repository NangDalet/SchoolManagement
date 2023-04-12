using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;

namespace SchoolManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult CreateStudent()
        {
            return View();
        }
        public IActionResult TestStu()
        {
            return View();
        }
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Code")))
            {
                return View();
            }
            return RedirectToAction("LogOut", "Login");
        }
        public async Task<IActionResult> PostStudent(ClsDeposit obj)
        {
            var isExists = _context.Deposits.SingleOrDefault(c => c.InvoiceNo == obj.InvoiceNo && c.ID != obj.ID);

            if (isExists != null)
                return BadRequest();

            var studentMaster = new ClsDeposit()
            {
                ID = obj.ID,
                InvoiceNo = obj.InvoiceNo,
                StuName = obj.StuName,
                StuPhone = obj.StuPhone,
                PostingDate = obj.PostingDate,
                Remark = obj.Remark,
                UserCode = obj.UserCode = HttpContext.Session.GetString("Code"),
                Total = obj.Total,
                Deposit = obj.Deposit,
                BalanceDue = obj.BalanceDue,
            };
            _context.Deposits.Add(studentMaster);
            await _context.SaveChangesAsync();

            foreach (var i in obj.lvDeposit)
            {
                var levelDetail = new LevelDeposit()
                {
                    ClsDepositId = studentMaster.ID,
                    LevelCode=i.LevelCode,
                    UnitPrice = i.UnitPrice,
                    DiscountPer = i.DiscountPer,
                    DiscountAmt = i.DiscountAmt,
                    DueDate = i.DueDate,
                    Time = i.Time,
                    Schedule = i.Schedule
                };
                _context.LevelDeposits.Add(levelDetail);
                await _context.SaveChangesAsync();
            }
            return Ok(obj);
        }
    }
}
