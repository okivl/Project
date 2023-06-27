using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.CreateUpdate
{
    public class BaseUserUpdate
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
