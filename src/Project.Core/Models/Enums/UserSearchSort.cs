using System.ComponentModel;

namespace Project.Core.Models.Enums
{
    public enum UserSearchSort
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
