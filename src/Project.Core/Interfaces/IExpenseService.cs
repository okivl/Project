using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;
using Project.Entities.Models;

namespace Project.Core.Interfaces
{
    public interface IExpenseService
    {
        Task<List<Expense>> AdminGetUserExpenses(DateRange dateRange, AdminIncomeExpenseSort sortBy, Pagination pagination, Guid? Id);
        Task<List<Expense>> GetAll(DateRange dateRange, IncomeExpenseSort sortBy, Pagination pagination);
        Task<Expense> Get(Guid Id);
        Task Create(ExpenseCU expenseCreate);
        Task Update(Guid Id, ExpenseCU expenseUpdate);
        Task Delete(Guid Id);
    }
}
