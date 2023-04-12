namespace SchoolManagement.Models
{
    public class LevelDeposit
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPer { get; set; }
        public decimal DiscountAmt { get; set; }
        public DateTime DueDate { get; set; } //ថ្ងៃមកចូលរៀន
        public string? Time { get; set; }
        public string? Schedule { get; set; }
        public string? LevelCode { get; set; }
        public int ClsDepositId { get; set; }
    }
}
