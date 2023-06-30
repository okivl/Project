using System.Security.Claims;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис токена
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерация Access токена
        /// </summary>
        /// <param name="claims">Пользовательские клеймы</param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Генерация Refresh токена
        /// </summary>
        string GenerateRefreshToken();

        /// <summary>
        /// Получение principal сущности
        /// </summary>
        /// <param name="token">Токен с истекшим сроком давности</param>
        /// <returns>principal сущность</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
