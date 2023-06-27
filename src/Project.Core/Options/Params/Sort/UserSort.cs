using System.ComponentModel;

namespace Project.Core.Options.Params.Sort
{
    public enum UserSort
    {
        [Description("NoSort")]
        None,
        [Description("Name")]
        Name,
        [Description("Surname")]
        Surname,
        [Description("DateOfBirth")]
        DateBirth,
        [Description("Email")]
        Email
    }
}
