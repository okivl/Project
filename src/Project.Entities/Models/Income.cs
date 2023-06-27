namespace Project.Entities.Models
{
    public class Income : BaseModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public IncomeSource IncomeSource { get; set; }
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
