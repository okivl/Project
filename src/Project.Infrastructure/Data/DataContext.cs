using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public virtual DbSet<IncomeSource> IncomeSources { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<User> Users { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
