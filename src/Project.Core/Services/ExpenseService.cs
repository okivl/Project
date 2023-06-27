using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public ExpenseService(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }



        public async Task<List<Expense>> AdminGetUserExpenses(DateRange dateRange, AdminIncomeExpenseSort sortBy, Pagination pagination, Guid? Id)
        {
            var expenses = _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .Where(i => i.CreateDate >= dateRange.DateFrom && i.CreateDate <= dateRange.DateTo);

            //if (Id != Guid.Empty)
            //{
            //    expenses = expenses.Where(i => i.User.Id == Id);
            //}

            switch (sortBy)
            {
                case AdminIncomeExpenseSort.None:
                    break;
                case AdminIncomeExpenseSort.Name:
                    expenses = expenses.OrderBy(i => i.Name);
                    break;
                case AdminIncomeExpenseSort.Amount:
                    expenses = expenses.OrderBy(i => i.Amount);
                    break;
                case AdminIncomeExpenseSort.Type:
                    expenses = expenses.OrderBy(i => i.ExpenseType.Name);
                    break;
                case AdminIncomeExpenseSort.User:
                    expenses = expenses.OrderBy(i => i.User.FirstName);
                    break;
            }

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                expenses = expenses.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

            return await expenses.ToListAsync();
        }

        public async Task<List<Expense>> GetAll(DateRange dateRange, IncomeExpenseSort sortBy, Pagination pagination)
        {
            var user = await _userService.GetUserInfo();

            var expenses = _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .Where(i => i.CreateDate >= dateRange.DateFrom && i.CreateDate <= dateRange.DateTo && i.User.Id == user.Id);

            switch (sortBy)
            {
                case IncomeExpenseSort.None:
                    break;
                case IncomeExpenseSort.Name:
                    expenses = expenses.OrderBy(i => i.Name);
                    break;
                case IncomeExpenseSort.Amount:
                    expenses = expenses.OrderBy(i => i.Amount);
                    break;
                case IncomeExpenseSort.Type:
                    expenses = expenses.OrderBy(i => i.ExpenseType.Name);
                    break;
            }

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                expenses = expenses.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

            return await expenses.ToListAsync();
        }

        public async Task<Expense> Get(Guid Id)
        {
            var expense = await _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Expense not found");

            return expense;
        }

        public async Task Create(ExpenseCU expenseCreate)
        {
            var user = await _userService.GetUserInfo();

            var expenseType = await _context.ExpenseTypes.FirstOrDefaultAsync(i => i.Id == expenseCreate.ExpenseTypeId)
                ?? throw new Exception("Type not found");

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Name = expenseCreate.Name,
                Amount = expenseCreate.Amount,
                ExpenseType = expenseType,
                User = user,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, ExpenseCU expenseUpdate)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(expenseUpdate.ExpenseTypeId)
                ?? throw new Exception("Type not found");

            var updateExpense = await _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Expense not found");

            updateExpense.ExpenseType = expenseType;

            updateExpense.Name = expenseUpdate.Name;
            updateExpense.Amount = expenseUpdate.Amount;
            updateExpense.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var expense = await _context.Expenses.FindAsync(Id) ?? throw new Exception("Expense not found");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}
