using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис пользователя
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <returns>Список всех пользователей</returns>
        Task<List<User>> GetAll(AdminUserSearchContext searchContext);

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User> Get(Guid Id);

        /// <summary>
        /// Получение информации пользователем о себе
        /// </summary>
        /// <returns>Пользователь</returns>
        Task<User> GetUserInfo();

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Параметры создания пользователя</param>
        Task Create(AdminUserCreateParameters user);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="Id">Идентификатор пользователя</param>
        /// <param name="user">Параметры обновления пользователя</param>
        Task Update(Guid Id, AdminUserUpdateParameters user);

        /// <summary>
        /// Обновление информации пользователем о себе
        /// </summary>
        /// <param name="userUpdate">Параметры обновления пользователя</param>
        Task UpdateUser(BaseUserUpdateParameters userUpdate);

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="Id">Идентификатор пользователя</param>
        /// <param name="password">Пароль</param>
        Task UpdatePassword(Guid Id, BaseUser password);

        /// <summary>
        /// Обновление пароля пользователем
        /// </summary>
        /// <param name="password">Пароль</param>
        Task UpdateUserPassword(BaseUser password);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="Id">Идентификатор пользователя</param>
        Task Delete(Guid Id);
    }
}
