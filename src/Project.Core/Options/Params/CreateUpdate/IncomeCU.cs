using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.CreateUpdate
{
    public class IncomeCU
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public Guid IncomeSourceId { get; set; }
    }
}
