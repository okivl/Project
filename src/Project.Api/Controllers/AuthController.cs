using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<BaseUser> _validator;
        private readonly IAuthService _authService;

        /// <summary/>
        public AuthController(IValidator<BaseUser> validator, IAuthService authService)
        {
            _validator = validator;
            _authService = authService;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Пароль</param>
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _authService.Login(email, password);

            return Ok(token);
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="userReg">Параметры регистрации</param>
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromQuery] UserRegParameters userReg)
        {
            await _validator.ValidateAndThrowAsync(userReg);
            var token = await _authService.Registr(userReg);

            return Ok(token);
        }

        /// <summary>
        /// Обновление токена
        /// </summary>
        /// <param name="accessToken">Основной токен</param>
        /// <param name="refreshToken">Токен обновления</param>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string accessToken, string refreshToken)
        {
            var token = await _authService.Refresh(accessToken, refreshToken);
            return Ok(token);
        }
    }
}
