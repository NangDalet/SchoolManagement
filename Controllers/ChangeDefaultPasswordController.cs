using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models.Connection;
using SchoolManagement.Models.Return;
using SchoolManagement.Models.Security;

namespace SchoolManagement.Controllers
{
    public class ChangeDefaultPasswordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChangeDefaultPasswordController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ChangeDefaultPassword()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("tmpCode")))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public async Task<IActionResult> ChangeDefaultPass(string NewPass)
        {
            ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var userUpdate = (from u in _context.Users
                                  where u.UserCode == HttpContext.Session.GetString("tmpCode")
                                  select u).FirstOrDefault();

                if (userUpdate != null)
                {
                    userUpdate.Password = Hashing.ToSHA256(NewPass);
                    userUpdate.Change = "Yes";
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Code", HttpContext.Session.GetString("tmpCode")); //tmpCode Focus on change password
                HttpContext.Session.SetString("tmpCode", ""); //clear session

                returnStatus.ErrCode = 0;
                returnStatus.ErrMsg = "Your password has been changed successfully.";
                
            }
            catch (Exception ex)
            {
                returnStatus.ErrCode = ex.HResult;
                returnStatus.ErrMsg = ex.Message;
            }
            return Ok(returnStatus);
        }
    }
}
