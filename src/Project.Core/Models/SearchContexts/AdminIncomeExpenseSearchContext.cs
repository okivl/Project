using Project.Core.Models.Enums;

namespace Project.Core.Models.SearchContexts
{
    public class AdminIncomeExpenseSearchContext : SearchContext
    {
        public Guid? Id { get; set; }
        public AdminIncomeExpenseSort Sort { get; set; }
        public OrderSort Order { get; set; }
    }
}
