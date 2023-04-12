using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using System.Drawing;
using static SchoolManagement.Models.ClsDeposit;

namespace SchoolManagement.Controllers
{
    public class DepositController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepositController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateDeposit()
        {
            //string Invoice = "DM202103000001";
            //string num="100";
            //num.PadLeft(7,'0'); //0000100
            return View();
        }
        public async Task<IActionResult> PostDeposit(ClsDeposit obj)
        {
            var isExists = _context.Deposits.SingleOrDefault(c => c.InvoiceNo == obj.InvoiceNo && c.ID != obj.ID);

            if (isExists != null)
                return BadRequest();

            //con.cmd.CommandText = "select count(*) as Num from tbLoan where Period in(YEAR('" + dtPosting.Text + "'))";
            //DateTime dt = PostingDate.DateTime;
            //string prefix = dt.Year.ToString().Substring(2);

            //int num = 0;
            //var reader = con.cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    num = Convert.ToInt32(reader[0]);
            //}
            ////22000001
            //num = num + 1;
            //string Number = num.ToString().PadLeft(5, '0');
            //string DocNum = prefix + Number;
            //var rows = from myRow in _context.Deposits
            //           where(per)
            //           select myRow;

            var studentMaster = new ClsDeposit()
            {
                ID = obj.ID,
                InvoiceNo = obj.InvoiceNo,
                StuName = obj.StuName,
                StuPhone = obj.StuPhone,
                PostingDate = obj.PostingDate,
                Remark = obj.Remark,
                UserCode =obj.UserCode= HttpContext.Session.GetString("Code"),
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
                    //levelCode = i.levelCode,
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
