using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.CreateUpdate
{
    public class BaseUser
    {
        [Required]
        public string Password { get; set; }
    }
}
