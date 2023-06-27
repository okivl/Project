using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.CreateUpdate
{
    public class ExpenseCU
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public Guid ExpenseTypeId { get; set; }
    }
}
