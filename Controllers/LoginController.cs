using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using SchoolManagement.Models.Return;
using SchoolManagement.Models.Security;
using System.Data;

namespace SchoolManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult CheckLogin(string usercode, string password)
        {
            ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var result = _context.Users.Where(m => m.UserCode == usercode && m.Password == Hashing.ToSHA256(password)).Select(u => new
                {
                    u.Id,
                    u.Status,
                    u.Change,
                    u.Locked,
                    u.ExpiredDate,
                    u.ExpiredLocked,
                    u.UserType,
                    u.Image,
                }).FirstOrDefault();

                if (result == null)
                {
                    returnStatus.ErrCode = 9999;
                    returnStatus.ErrMsg = "Username and Password Invalid, Please try again !";
                }
                else if(result.Status== "InActive")
                {
                    returnStatus.ErrCode = 1;
                    returnStatus.ErrMsg = "User Is InActive, please contact to administration !";
                }
                else if (result.Locked == "Yes")
                {
                    returnStatus.ErrCode = 2;
                    returnStatus.ErrMsg = "User is locked, please contact to administration !";
                }
                else if (result.Change == "No" && result.ExpiredDate < DateTime.Now)
                {
                    returnStatus.ErrCode = 3;
                    returnStatus.ErrMsg = "User is Expired, please contact to administration !";
                }
                else if (result.Change == "No")
                {
                    HttpContext.Session.SetString("tmpCode", usercode); //use for change default password when new user
                    returnStatus.ErrCode = 4;
                    returnStatus.ErrMsg = "";
                }
                else
                {
                    HttpContext.Session.SetString("Code", usercode); //code focus proted all function
                    HttpContext.Session.SetString("Id", result?.Id.ToString());
                    HttpContext.Session.SetString("UrlImage",result.Image);
                    HttpContext.Session.SetString("UserType", result.UserType);
                }
            }
            catch (Exception ex)
            {
                returnStatus.ErrCode = ex.HResult;
                returnStatus.ErrMsg = ex.Message;
            }
            return Ok(returnStatus);
        }   
        public async Task<IActionResult> ChangePassword(string OldPass, string NewPass)
        {
            ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var changePassword = (from u in _context.Users
                                  where u.UserCode == HttpContext.Session.GetString("Code") && u.Password==Hashing.ToSHA256(OldPass) 
                                  select u).FirstOrDefault();
                if (returnStatus.ErrCode == 0)
                {
                    changePassword.Password = Hashing.ToSHA256(NewPass);
                }
                await _context.SaveChangesAsync();
                //if (changePassword != null)
                //{
                //    changePassword.Password = Hashing.ToSHA256(NewPass);
                //}
                //await _context.SaveChangesAsync();
                //returnStatus.ErrCode = 0;
                //returnStatus.ErrMsg = "Your password has been changed successfully.";
            }
            catch (Exception ex)
            {
                returnStatus.ErrCode = ex.HResult;
                returnStatus.ErrMsg = ex.Message;
            }
            return Ok(returnStatus);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
