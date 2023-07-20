using Project.Core.Models.Enums;

namespace Project.Core.Models.SearchContexts
{
    public class IncomeExpenseSearchContext : SearchContext
    {
        public IncomeExpenseSearchSort Sort { get; set; }
        public OrderSort Order { get; set; }
    }
}
