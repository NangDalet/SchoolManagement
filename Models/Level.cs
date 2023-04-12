namespace SchoolManagement.Models
{
    public class Level
    {
        public int Id { get; set; }
        public string? LevelCode { get; set; }
        public string? LevelName { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Status { get; set; }
    }
}
