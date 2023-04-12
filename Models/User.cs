using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? UserCode { get; set; } //==username
        public string? Name { get; set; }
        public string? UserType { get; set; }
        public string? Password { get; set; }
        public string? Locked { get; set; }
        public string? Change { get; set; }
        public DateTime CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string? ExpiredLocked { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }

    }
}
