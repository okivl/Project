using Project.Core.Models.Abstractions;
using Project.Core.Models.Enums;

namespace Project.Core.Models.SearchContexts
{
    public class AdminUserSearchContext : Pagination
    {
        public UserSearchSort Sort { get; set; }
        public OrderSort Order { get; set; }
    }
}
