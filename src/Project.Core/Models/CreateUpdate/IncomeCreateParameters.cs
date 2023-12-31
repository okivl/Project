﻿using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.CreateUpdate
{
    public class IncomeCreateParameters
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public Guid IncomeSourceId { get; set; }
    }
}
