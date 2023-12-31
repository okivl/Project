﻿using System.ComponentModel.DataAnnotations;

namespace Project.Core.Models.CreateUpdate
{
    public class ExpenseCreateParameters
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public Guid ExpenseTypeId { get; set; }
    }
}
