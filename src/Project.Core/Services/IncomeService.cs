using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public IncomeService(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Income>> AdminGetUserIncomes(DateRange dateRange, AdminIncomeExpenseSort sortBy, Pagination pagination, Guid? Id)
        {
            var incomes = _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .Where(i => i.CreateDate >= dateRange.DateFrom && i.CreateDate <= dateRange.DateTo);

            //if (Id != Guid.Empty)
            //{
            //    incomes = incomes.Where(i => i.User.Id == Id);
            //}

            switch (sortBy)
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

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                incomes = incomes.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

            return await incomes.ToListAsync();
        }

        public async Task<List<Income>> GetAll(DateRange dateRange, IncomeExpenseSort sortBy, Pagination pagination)
        {
            var user = await _userService.GetUserInfo();

            var incomes = _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .Where(i => i.CreateDate >= dateRange.DateFrom && i.CreateDate <= dateRange.DateTo && i.User.Id == user.Id);

            switch (sortBy)
            {
                case IncomeExpenseSort.None:
                    break;
                case IncomeExpenseSort.Name:
                    incomes = incomes.OrderBy(i => i.Name);
                    break;
                case IncomeExpenseSort.Amount:
                    incomes = incomes.OrderBy(i => i.Amount);
                    break;
                case IncomeExpenseSort.Type:
                    incomes = incomes.OrderBy(i => i.IncomeSource.Name);
                    break;
            }

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                incomes = incomes.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

            return await incomes.ToListAsync();
        }

        public async Task<Income> Get(Guid Id)
        {
            var income = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == Id)
                ?? throw new Exception("Income not found");

            return income;
        }

        public async Task Create(IncomeCU incomeCreate)
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
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, IncomeCU incomeUpdate)
        {
            var incomeSource = await _context.IncomeSources.FindAsync(incomeUpdate.IncomeSourceId)
                ?? throw new Exception("Source not found");

            var updateIncome = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Income not found");

            updateIncome.IncomeSource = incomeSource;

            updateIncome.Name = incomeUpdate.Name;
            updateIncome.Amount = incomeUpdate.Amount;
            updateIncome.UpdateDate = DateTime.UtcNow;

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
