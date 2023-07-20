using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.CreateUpdate
{
    public class BaseUser
    {
        [Required]
        public string Password { get; set; }
    }
}
