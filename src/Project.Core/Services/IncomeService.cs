using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.Enums;
using Project.Core.Models.SearchContexts;
using Project.Entities;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IIncomeService"/>
    public class IncomeService : IIncomeService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public IncomeService(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Income>> AdminGetUserIncomes(AdminIncomeExpenseSearchContext searchContext)
        {
            var incomes = _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .Where(i => i.CreatedAt >= searchContext.DateFrom && i.CreatedAt <= searchContext.DateTo);

            if (searchContext.Id != Guid.Empty)
            {
                incomes = incomes.Where(i => i.User.Id == searchContext.Id);
            }

            switch (searchContext.Sort)
            {
                case AdminIncomeExpenseSort.None:
                    break;
                case AdminIncomeExpenseSort.Name:
                    incomes = incomes.OrderBy(i => i.Name);
                    break;
                case AdminIncomeExpenseSort.Amount:
                    incomes = incomes.OrderBy(i => i.Amount);
                    break;
                case AdminIncomeExpenseSort.Type:
                    incomes = incomes.OrderBy(i => i.IncomeSource.Name);
                    break;
                case AdminIncomeExpenseSort.User:
                    incomes = incomes.OrderBy(i => i.User.FirstName);
                    break;
            }

            incomes = searchContext.Order == OrderSort.Ascending ? incomes : incomes.Reverse();

            incomes = incomes.Skip((searchContext.Page - 1) * searchContext.PageSize).Take(searchContext.PageSize);

            return await incomes.ToListAsync();
        }

        public async Task<List<Income>> GetAll(IncomeExpenseSearchContext searchContext)
        {
            var user = await _userService.GetUserInfo();

            var incomes = _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .Where(i => i.CreatedAt >= searchContext.DateFrom && i.CreatedAt <= searchContext.DateTo && i.User.Id == user.Id);

            switch (searchContext.Sort)
            {
                case IncomeExpenseSearchSort.None:
                    break;
                case IncomeExpenseSearchSort.Name:
                    incomes = incomes.OrderBy(i => i.Name);
                    break;
                case IncomeExpenseSearchSort.Amount:
                    incomes = incomes.OrderBy(i => i.Amount);
                    break;
                case IncomeExpenseSearchSort.Type:
                    incomes = incomes.OrderBy(i => i.IncomeSource.Name);
                    break;
            }
            incomes = searchContext.Order == OrderSort.Ascending ? incomes : incomes.Reverse();

            incomes = incomes.Skip((searchContext.Page - 1) * searchContext.PageSize).Take(searchContext.PageSize);

            return await incomes.ToListAsync();
        }

        public async Task<Income> Get(Guid Id)
        {
            var income = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == Id)
                ?? throw new Exception("Income not found");

            return income;
        }

        public async Task Create(IncomeCraeteUpdateParameters incomeCreate)
        {
            var user = await _userService.GetUserInfo();

            var incomeSource = await _context.IncomeSources.FirstOrDefaultAsync(i => i.Id == incomeCreate.IncomeSourceId)
                ?? throw new Exception("Source not found");

            var income = new Income
            {
                Id = Guid.NewGuid(),
                Name = incomeCreate.Name,
                Amount = incomeCreate.Amount,
                IncomeSource = incomeSource,
                User = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, IncomeCraeteUpdateParameters incomeUpdate)
        {
            var incomeSource = await _context.IncomeSources.FindAsync(incomeUpdate.IncomeSourceId)
                ?? throw new Exception("Source not found");

            var updateIncome = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Income not found");

            updateIncome.IncomeSource = incomeSource;

            updateIncome.Name = incomeUpdate.Name;
            updateIncome.Amount = incomeUpdate.Amount;
            updateIncome.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var income = await _context.Incomes.FindAsync(Id) ?? throw new Exception("Income not found");

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
        }
    }
}
