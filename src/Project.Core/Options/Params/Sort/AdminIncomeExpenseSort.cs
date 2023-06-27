using System.ComponentModel;

namespace Project.Core.Options.Params.Sort
{
    public enum AdminIncomeExpenseSort
    {
        [Description("NoSort")]
        None,
        [Description("Name")]
        Name,
        [Description("Amount")]
        Amount,
        [Description("Type")]
        Type,
        [Description("User")]
        User
    }
}
