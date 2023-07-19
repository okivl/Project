using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис расходов
    /// </summary>
    public interface IExpenseService
    {
        /// <summary>
        /// Получение всех расходов пользователей
        /// </summary>
        /// <param name="searchContext">Парметры поиска</param>
        /// <returns>Список расходов пользователей</returns>
        Task<List<Expense>> GetAll(AdminIncomeExpenseSearchContext searchContext);

        /// <summary>
        /// Получение всех расходов пользователем
        /// </summary>
        /// <param name="searchContext">Парметры поиска</param>
        /// <returns>Список расходов пользователея</returns>
        Task<List<Expense>> GetUserExpenses(IncomeExpenseSearchContext searchContext);

        /// <summary>
        /// Получение расхода по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор расхода</param>
        /// <returns>Расход</returns>
        Task<Expense> Get(Guid Id);

        /// <summary>
        /// Создание расхода
        /// </summary>
        /// <param name="expenseCreate">Параметры создания расхода</param>
        Task Create(ExpenseCreateParameters expenseCreate);

        /// <summary>
        /// Обновление расхода
        /// </summary>
        /// <param name="Id">Идентификатор расхода</param>
        /// <param name="expenseUpdate">Параметры редактирования расхода</param>
        Task Update(Guid Id, ExpenseUpdateParameters expenseUpdate);

        /// <summary>
        /// Удаление расхода
        /// </summary>
        /// <param name="Id">Идентификатор расхода</param>
        Task Delete(Guid Id);
    }
}
