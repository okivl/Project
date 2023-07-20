using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис источника дохода
    /// </summary>
    public interface IIncomeSourceService
    {
        /// <summary>
        /// Получение всех источников дохода
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <returns>Список источников дохода</returns>
        Task<List<IncomeSource>> GetAll(IncomeExpenseTypeSearchContext searchContext);

        /// <summary>
        /// Получение источника дохода по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор источника дохода</param>
        /// <returns>Источник дохода</returns>
        Task<IncomeSource> Get(Guid Id);

        /// <summary>
        /// Создание источника дохода
        /// </summary>
        /// <param name="Name">Наименование источника дохода</param>
        Task Create(string Name);

        /// <summary>
        /// Обновление источника дохода
        /// </summary>
        /// <param name="Id">Идентификатор источника дохода</param>
        /// <param name="Name">Наименование источника дохода</param>
        Task Update(Guid Id, string Name);

        /// <summary>
        /// Удаление источника дохода
        /// </summary>
        /// <param name="Id">Идентификатор источника дохода</param>
        Task Delete(Guid Id);
    }
}
