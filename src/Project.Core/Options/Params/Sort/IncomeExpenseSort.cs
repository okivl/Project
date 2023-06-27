using System.ComponentModel;

namespace Project.Core.Options.Params.Sort
{
    public enum IncomeExpenseSort
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
