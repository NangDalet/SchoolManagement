namespace SchoolManagement.Models.Return
{
    public class ReturnLevel
    {
        public int ErrCode { get; set; }
        public string? ErrMsg { get; set; }
        public List<Level>? levels { get; set; }
    }
}
