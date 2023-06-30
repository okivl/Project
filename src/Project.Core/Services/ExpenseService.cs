﻿using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.Enums;
using Project.Core.Models.SearchContexts;
using Project.Entities;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IExpenseService"/>
    public class ExpenseService : IExpenseService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public ExpenseService(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }



        public async Task<List<Expense>> AdminGetUserExpenses(AdminIncomeExpenseSearchContext searchContext)
        {
            var expenses = _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .Where(i => i.CreatedAt >= searchContext.DateFrom && i.CreatedAt <= searchContext.DateTo);

            if (searchContext.Id != Guid.Empty)
            {
                expenses = expenses.Where(i => i.User.Id == searchContext.Id);
            }

            switch (searchContext.Sort)
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

            expenses = searchContext.Order == OrderSort.Ascending ? expenses : expenses.Reverse();

            expenses = expenses.Skip((searchContext.Page - 1) * searchContext.PageSize).Take(searchContext.PageSize);

            return await expenses.ToListAsync();
        }

        public async Task<List<Expense>> GetAll(IncomeExpenseSearchContext searchContext)
        {
            var user = await _userService.GetUserInfo();

            var expenses = _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .Where(i => i.CreatedAt >= searchContext.DateFrom && i.CreatedAt <= searchContext.DateTo && i.User.Id == user.Id);

            switch (searchContext.Sort)
            {
                case IncomeExpenseSearchSort.None:
                    break;
                case IncomeExpenseSearchSort.Name:
                    expenses = expenses.OrderBy(i => i.Name);
                    break;
                case IncomeExpenseSearchSort.Amount:
                    expenses = expenses.OrderBy(i => i.Amount);
                    break;
                case IncomeExpenseSearchSort.Type:
                    expenses = expenses.OrderBy(i => i.ExpenseType.Name);
                    break;
            }

            expenses = searchContext.Order == OrderSort.Ascending ? expenses : expenses.Reverse();

            expenses = expenses.Skip((int)((searchContext.Page - 1) * searchContext.PageSize)).Take((int)searchContext.PageSize);

            return await expenses.ToListAsync();
        }

        public async Task<Expense> Get(Guid Id)
        {
            var expense = await _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Expense not found");

            return expense;
        }

        public async Task Create(ExpenseCreateUpdateParameters expenseCreate)
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, ExpenseCreateUpdateParameters expenseUpdate)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(expenseUpdate.ExpenseTypeId)
                ?? throw new Exception("Type not found");

            var updateExpense = await _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Expense not found");

            updateExpense.ExpenseType = expenseType;

            updateExpense.Name = expenseUpdate.Name;
            updateExpense.Amount = expenseUpdate.Amount;
            updateExpense.UpdatedAt = DateTime.UtcNow;

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
