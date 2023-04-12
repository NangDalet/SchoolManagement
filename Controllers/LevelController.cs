using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using SchoolManagement.Models.Return;
using SchoolManagement.Models.Security;
using System.Linq;
using System.Xml.Linq;

namespace SchoolManagement.Controllers
{
    public class LevelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LevelController(ApplicationDbContext context)
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
        public async Task<IActionResult> GetAllLevel()
        {
            return Ok(new { data = await _context.Levels.ToListAsync()});
        }
        //public async Task<IActionResult> GetAllLevel()
        //{
        //    return Ok(new { data = await _context.Levels.ToListAsync() });
        //}
        [HttpPost]
        public async Task<IActionResult> Create(Level Obj)
        {
            ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var isExists = _context.Levels.SingleOrDefault(c => c.LevelCode == Obj.LevelCode || c.LevelName == Obj.LevelName);

                if (isExists != null)
                    return BadRequest();

                var level = new Level
                {
                    LevelCode = Obj.LevelCode,
                    LevelName = Obj.LevelName,
                    UnitPrice = Obj.UnitPrice,
                    Status = Obj.Status,
                };
                _context.Levels.Add(Obj);
                await _context.SaveChangesAsync();

                returnStatus.ErrCode = 0;
                returnStatus.ErrMsg = "This level has been saved successfully.";
            }
            catch (Exception ex)
            {
                returnStatus.ErrCode = ex.HResult;
                returnStatus.ErrMsg = ex.Message;
            }
            return Ok(Obj);
        }
        //GET:level/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            var level = await _context.Levels.FindAsync(id);
            //Product product = await _context.Product.FindAsync(id);
            return Ok(level);
        }
        public async Task<IActionResult> Update(Level level)
        {
            ReturnStatus returnStatus = new ReturnStatus();
            try
            {
                var isExists = _context.Levels.SingleOrDefault(c => c.LevelName == level.LevelName && c.Id != level.Id);
                if (isExists != null)
                    return BadRequest();

                var levelUpdate = (from l in _context.Levels
                                   where l.Id == level.Id
                                   select l).FirstOrDefault();

                if (levelUpdate != null)
                {
                    levelUpdate.LevelName = level.LevelName;
                    levelUpdate.UnitPrice = level.UnitPrice;
                    levelUpdate.Status = level.Status;
                    await _context.SaveChangesAsync();
                } 
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(level);
        }
        public IActionResult Delete(int id)
        {
            var lev = _context.Levels.FirstOrDefault(c => c.Id == id);
            if(lev != null)
            {
                _context.Levels.RemoveRange(lev);
                _context.SaveChanges();
            }
            return Ok(lev);
        }
    }
}
