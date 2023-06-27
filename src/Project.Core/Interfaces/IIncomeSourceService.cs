using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;

namespace Project.Core.Interfaces
{
    public interface IIncomeSourceService
    {
        Task<List<IncomeSource>> GetAll(Pagination pagination, TypeSort sortBy);
        Task<IncomeSource> Get(Guid Id);
        Task Create(string Name);
        Task Update(Guid Id, string Name);
        Task Delete(Guid Id);
    }
}
