using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Models.Enums;
using Project.Core.Models.SearchContexts;
using Project.Entities;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IExpenseTypeService"/>
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly DataContext _context;

        public ExpenseTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseType>> GetAll(IncomeExpenseTypeSearchContext searchContext)
        {
            var types = _context.ExpenseTypes.AsQueryable();

            switch (searchContext.Sort)
            {
                case TypeSearchSort.None:
                    break;
                case TypeSearchSort.Name:
                    types = types.OrderBy(i => i.Name);
                    break;
            }

            types = searchContext.Order == OrderSort.Ascending ? types : types.Reverse();

            types = types.Skip(((searchContext.Page - 1) * searchContext.PageSize)).Take(searchContext.PageSize);

            return await types.ToListAsync();
        }

        public async Task<ExpenseType> Get(Guid Id)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(Id) ?? throw new Exception("Type not found");

            return expenseType;
        }

        public async Task Create(string name)
        {
            var expenseType = new ExpenseType
            {
                Id = Guid.NewGuid(),
                Name = name,
            };
            _context.ExpenseTypes.Add(expenseType);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, string name)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(Id) ?? throw new Exception("Type not found");

            expenseType.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var expenseType = await _context.ExpenseTypes.FindAsync(Id) ?? throw new Exception("Invalid access token");

            _context.ExpenseTypes.Remove(expenseType);
            await _context.SaveChangesAsync();
        }
    }
}
