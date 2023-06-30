using Project.Core.Models;
using Project.Core.Models.CreateUpdate;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="email">emailparam>
        /// <param name="password">Пароль</param>
        /// <returns>Access и Refresh токены</returns>
        Task<AuthResponseDto> Login(string email, string password);

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="userReg">Параметры авторизации</param>
        /// <returns>Access и Refresh токены</returns>
        Task<AuthResponseDto> Registr(UserRegParameters userReg);

        /// <summary>
        /// Обновление просроченного токена доступа
        /// </summary>
        /// <param name="accessToken">Access токен</param>
        /// <param name="refreshToken">Refresh токен</param>
        /// <returns>Access и Refresh токены</returns>
        Task<AuthResponseDto> Refresh(string accessToken, string refreshToken);
    }
}
