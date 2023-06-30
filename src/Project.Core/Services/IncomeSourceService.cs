using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Models.Enums;
using Project.Core.Models.SearchContexts;
using Project.Entities;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IIncomeSourceService"/>
    public class IncomeSourceService : IIncomeSourceService
    {
        private readonly DataContext _context;

        public IncomeSourceService(DataContext context)
        {
            _context = context;
        }


        public async Task<List<IncomeSource>> GetAll(IncomeExpenseTypeSearchContext searchContext)
        {
            var sources = _context.IncomeSources.AsQueryable();

            switch (searchContext.Sort)
            {
                case TypeSearchSort.None:
                    break;
                case TypeSearchSort.Name:
                    sources = sources.OrderBy(i => i.Name);
                    break;
            }

            sources = searchContext.Order == OrderSort.Ascending ? sources : sources.Reverse();

            sources = sources.Skip((searchContext.Page - 1) * searchContext.PageSize).Take(searchContext.PageSize);

            return await sources.ToListAsync();
        }

        public async Task<IncomeSource> Get(Guid Id)
        {
            var incomeSource = await _context.IncomeSources
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Source not found");

            return incomeSource;
        }

        public async Task Create(string name)
        {

            var incomeSource = new IncomeSource
            {
                Id = Guid.NewGuid(),
                Name = name,
            };

            _context.IncomeSources.Add(incomeSource);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid Id, String name)
        {
            var incomeSource = await _context.IncomeSources
                .FirstOrDefaultAsync(i => i.Id == Id) ?? throw new Exception("Source not found");

            incomeSource.Name = name;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var incomeSource = await _context.IncomeSources.FindAsync(Id) ?? throw new Exception("Source not found");

            _context.IncomeSources.Remove(incomeSource);

            await _context.SaveChangesAsync();
        }
    }
}
