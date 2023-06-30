using Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.CreateUpdate
{
    public class AdminUserUpdateParameters : BaseUserUpdateParameters
    {
        [Required]
        public Roles Role { get; set; }
    }
}
