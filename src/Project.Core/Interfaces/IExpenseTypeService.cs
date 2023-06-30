using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис создания категории расхода
    /// </summary>
    public interface IExpenseTypeService
    {
        /// <summary>
        /// Получение всех категорий расхода
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <returns>Список категорий расхода</returns>
        Task<List<ExpenseType>> GetAll(IncomeExpenseTypeSearchContext searchContext);

        /// <summary>
        /// Получение категориии расхода по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор категории расхода</param>
        /// <returns>Категория расхода</returns>
        Task<ExpenseType> Get(Guid Id);

        /// <summary>
        /// Создание категории расхода
        /// </summary>
        /// <param name="Name">Наименование категории расхода</param>
        Task Create(string Name);

        /// <summary>
        /// Обновление категории расхода
        /// </summary>
        /// <param name="Id">Идентификатор категории расхода</param>
        /// <param name="Name">Наименование категории расхода</param>
        Task Update(Guid Id, string Name);

        /// <summary>
        /// Удаление категории расхода
        /// </summary>
        /// <param name="Id">Идентификатор категории расхода</param>
        Task Delete(Guid Id);
    }
}
