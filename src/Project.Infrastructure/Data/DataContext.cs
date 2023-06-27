using Microsoft.EntityFrameworkCore;
using Project.Entities.Models;

namespace Project.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DbSet<IncomeSource> IncomeSources { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
