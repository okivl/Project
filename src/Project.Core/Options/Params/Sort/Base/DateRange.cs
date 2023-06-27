using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.Sort.Base
{
    public class DateRange
    {
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;
    }
}
