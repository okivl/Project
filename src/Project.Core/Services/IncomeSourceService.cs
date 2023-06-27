using Microsoft.EntityFrameworkCore;
using Project.Core.Interfaces;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    public class IncomeSourceService : IIncomeSourceService
    {
        private readonly DataContext _context;

        public IncomeSourceService(DataContext context)
        {
            _context = context;
        }


        public async Task<List<IncomeSource>> GetAll(Pagination pagination, TypeSort sortBy)
        {

            var sources = _context.IncomeSources.AsQueryable();

            switch (sortBy)
            {
                case TypeSort.None:
                    break;
                case TypeSort.Name:
                    sources = sources.OrderBy(i => i.Name);
                    break;
            }

            if (pagination.Page.HasValue && pagination.PageSize.HasValue)
            {
                sources = sources.Skip((int)((pagination.Page - 1) * pagination.PageSize)).Take((int)pagination.PageSize);
            }

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
