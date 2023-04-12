using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using SchoolManagement.Models.Return;
using SchoolManagement.Models.Security;

namespace SchoolManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Code")))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        //public IActionResult GetAllUser()
        //{
        //    var statistic = from u in _context.Users
        //                    select new
        //                    {
        //                       u.UserCode,
        //                       u.CreateDate.ToShortDateString("")
        //                        Number = g.Count()
        //                    };
        //    return Ok(statistic);
        //}
        public async Task<IActionResult> GetAllUser(User obj)
        {
            return Ok(new { data = await _context.Users.ToListAsync() });
            //ReturnStatus returnStatus = new ReturnStatus();
            //try
            //{
            //    var result = _context.Users.Where(m => m.UserCode == obj.UserCode).Select(u => new
            //    {
            //        u.Id,
            //        u.Status,
            //        u.UserCode,
            //        u.Name,
            //        u.EmpCode,
            //        u.Change,
            //        u.Locked,
            //        u.CreateDate,
            //        u.CreateBy,
            //        u.ExpiredDate,
            //        u.ExpiredLocked,
            //        u.Image,
            //    }).FirstOrDefault();

            //    if (obj.Change == "No" && result.ExpiredDate < DateTime.Now)
            //    {
            //        obj.ExpiredLocked = "Yes";
            //    }
            //    else
            //    {
            //        obj.ExpiredLocked = "No";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    returnStatus.ErrCode = ex.HResult;
            //    returnStatus.ErrMsg = ex.Message;
            //}
            //return Ok(new { data = await _context.Users.ToListAsync() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(User Obj)
        {
            try
            {
                DateTime dt = DateTime.Now;

                var isExists = _context.Users.SingleOrDefault(c => c.UserCode == Obj.UserCode && c.Id != Obj.Id);

                if (isExists != null)
                    return BadRequest();

                var user = new User
                {
                    UserCode = Obj.UserCode,
                    Name = Obj.Name,
                    Password = Obj.Password = Hashing.ToSHA256("TEST12345"),
                    Locked = Obj.Locked,
                    Change = Obj.Change = "No",
                    CreateDate = Obj.CreateDate=dt,
                    CreateBy = Obj.CreateBy = HttpContext.Session.GetString("Code"),
                    ExpiredDate = Obj.ExpiredDate=dt.AddDays(2),
                    ExpiredLocked=Obj.ExpiredLocked="No",
                    Status = Obj.Status,
                    Image=Obj.Image
                };
                _context.Users.Add(Obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(Obj);
        }
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            User Obj = new User();

            var file = HttpContext.Request.Form.Files;
            string path = "";
            string filePath = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/User/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var f in file)
            {
                filePath = path + f.FileName;
                using (Stream fs = System.IO.File.Create(filePath))
                {
                    await f.CopyToAsync(fs);
                    fs.Close();
                }
            }
            return Ok(Obj);
        }
        //GET:User/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _context.Users.FindAsync(id);
            return Ok(user);
        }
        public async Task<IActionResult> Update(User user)
        {
            //ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var isExists = _context.Users.SingleOrDefault(c => c.UserCode == user.UserCode && c.Id != user.Id);
                if (isExists != null)
                    return BadRequest();

                if (user.Image == null)
                {
                    var userUpdate = (from u in _context.Users
                                      where u.Id == user.Id
                                      select u).FirstOrDefault();

                    if (userUpdate != null)
                    {
                        userUpdate.Name = user.Name;
                        userUpdate.Locked = user.Locked;
                        userUpdate.Status = user.Status;                     
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    var userUpdate = (from u in _context.Users
                                      where u.Id == user.Id
                                      select u).FirstOrDefault();

                    if (userUpdate != null)
                    {
                        userUpdate.Name = user.Name;
                        userUpdate.Locked = user.Locked;
                        userUpdate.Status = user.Status;
                        userUpdate.Image = user.Image;
                        await _context.SaveChangesAsync();
                    }
                }
                //return Ok(levelUpdate);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(user);
        }
        public async Task<IActionResult> ResetPass(User user)
        {
            //ReturnStatus returnStatus = new ReturnStatus();
            DateTime dt = DateTime.Now;
            try
            {
                var isExists = _context.Users.SingleOrDefault(c => c.UserCode == user.UserCode && c.Id != user.Id);
                if (isExists != null)
                    return BadRequest();

                var userUpdate = (from u in _context.Users
                                  where u.Id == user.Id
                                  select u).FirstOrDefault();

                if (userUpdate != null)
                {
                    userUpdate.Password = user.Password = Hashing.ToSHA256("TEST12345");
                    userUpdate.Change = user.Change = "No";
                    userUpdate.ExpiredDate = user.ExpiredDate=dt.AddDays(2);
                    await _context.SaveChangesAsync();
                }
                //return Ok(levelUpdate);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(user);
        }
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                _context.Users.RemoveRange(user);
                _context.SaveChanges();
            }
            return Ok(user);
        }
    }
}
