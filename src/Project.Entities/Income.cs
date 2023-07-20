using Project.Entities.Abstractions;

namespace Project.Entities
{
    public class Income : BaseModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public IncomeSource IncomeSource { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
