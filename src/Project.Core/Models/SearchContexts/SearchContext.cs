using Project.Core.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.SearchContexts
{
    public class SearchContext : Pagination
    {
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;
    }
}
