using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly DataContext _context;

        public ExpenseTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseType>> GetAll(Pagination pagination, TypeSort sortBy)
        {
            var types = _context.ExpenseTypes.AsQueryable();

            switch (sortBy)
            {
                case TypeSort.None:
                    break;
                case TypeSort.Name:
                    types = types.OrderBy(i => i.Name);
                    break;
            }

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                types = types.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

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
