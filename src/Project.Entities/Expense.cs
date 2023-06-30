using Project.Entities.Abstractions;

namespace Project.Entities
{
    public class Expense : BaseModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
