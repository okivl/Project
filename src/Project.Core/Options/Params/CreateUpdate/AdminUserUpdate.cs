using Project.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Options.Params.CreateUpdate
{
    public class AdminUserUpdate : BaseUserUpdate
    {
        [Required]
        public Roles Role { get; set; }
    }
}
