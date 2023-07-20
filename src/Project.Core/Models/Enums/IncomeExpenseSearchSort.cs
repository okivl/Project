using System.ComponentModel;

namespace Project.Core.Models.Enums
{
    public enum IncomeExpenseSearchSort
    {
        [Description("NoSort")]
        None,
        [Description("Name")]
        Name,
        [Description("Amount")]
        Amount,
        [Description("Type")]
        Type
    }
}
