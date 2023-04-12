using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class ClsDeposit
    {
        public int ID { get; set; }
        public string? InvoiceNo { get; set; }
        public string? StuName { get; set; }
        public string? StuPhone { get; set; }
        public DateTime PostingDate { get; set; }= DateTime.Now;
        public string? Remark { get; set; }
        public string? UserCode { get; set; }
        public decimal Total { get; set; }
        public decimal Deposit { get; set; }
        public decimal BalanceDue { get; set; } //លុយនៅសល់
        public virtual List<LevelDeposit>? lvDeposit { get; set; }
        //Detail
        //public class LevelDeposit
        //{
        //    public Guid Id { get; set; }
        //    public decimal UnitPrice { get; set; }
        //    public decimal DiscountPer { get; set; }
        //    public decimal DiscountAmt { get; set; }
        //    public DateTime DueDate { get; set; } //ថ្ងៃមកចូលរៀន
        //    public string? Time { get; set; }
        //    public string? Schedule { get; set; }
        //    public Guid ClsDepositId { get; set; }
        //}
    }
}
