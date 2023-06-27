using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;

namespace Project.Core.Interfaces
{
    public interface IIncomeService
    {
        Task<List<Income>> AdminGetUserIncomes(DateRange dateRange, AdminIncomeExpenseSort sortBy, Pagination pagination, Guid? Id);
        Task<List<Income>> GetAll(DateRange dateRange, IncomeExpenseSort sortBy, Pagination pagination);
        Task<Income> Get(Guid Id);
        Task Create(IncomeCU incomeCreate);
        Task Update(Guid Id, IncomeCU incomeUpdate);
        Task Delete(Guid Id);
    }
}
