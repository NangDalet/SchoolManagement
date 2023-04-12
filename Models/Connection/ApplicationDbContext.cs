using Microsoft.EntityFrameworkCore;

namespace SchoolManagement.Models.Connection
{
    public class ApplicationDbContext:DbContext
    {
        //Member
        //private int ErrCode;
        //private string ErrMsg;
        //Property
        //public int _ErrCode { get { return ErrCode; } } //read only
        //public string _ErrMsg { get { return ErrMsg; } } //read only
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ClsDeposit> Deposits { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<LevelDeposit> LevelDeposits { get; set; }
    }
}
