using Project.Core.Models.Abstractions;
using Project.Core.Models.Enums;

namespace Project.Core.Models.SearchContexts
{
    public class IncomeExpenseTypeSearchContext : Pagination
    {
        public TypeSearchSort Sort { get; set; }
        public OrderSort Order { get; set; }
    }
}
