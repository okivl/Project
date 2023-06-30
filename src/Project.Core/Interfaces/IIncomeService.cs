using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис дохода
    /// </summary>
    public interface IIncomeService
    {
        /// <summary>
        /// Получение всех доходов пользователей
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <returns>Список доходов пользователей</returns>
        Task<List<Income>> AdminGetUserIncomes(AdminIncomeExpenseSearchContext searchContext);

        /// <summary>
        /// Получение всех доходов пользователем
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <returns>Список доходов пользователя</returns>
        Task<List<Income>> GetAll(IncomeExpenseSearchContext searchContext);

        /// <summary>
        /// Получение дохода по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор дохода</param>
        /// <returns>Доход</returns>
        Task<Income> Get(Guid Id);

        /// <summary>
        /// Создание нового дохода
        /// </summary>
        /// <param name="incomeCreate">Параметры создания дохода</param>
        /// <returns></returns>
        Task Create(IncomeCraeteUpdateParameters incomeCreate);

        /// <summary>
        /// Обновление дохода
        /// </summary>
        /// <param name="Id">Идентификатор дохода</param>
        /// <param name="incomeUpdate"></param>
        Task Update(Guid Id, IncomeCraeteUpdateParameters incomeUpdate);

        /// <summary>
        /// Удаление дохода
        /// </summary>
        /// <param name="Id">Идентификатор дохода</param>
        Task Delete(Guid Id);
    }
}
