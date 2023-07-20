using Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.CreateUpdate
{
    public class AdminUserCreateParameters : BaseUser
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
        [Required]
        public string Login { get; set; }
        [Required]
        public Roles Role { get; set; }
    }
}
