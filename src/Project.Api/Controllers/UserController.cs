using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models;
using Project.Core.Models.CreateUpdate;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<BaseUser> _validator;

        /// <summary/>
        public UserController(IValidator<BaseUser> validator, IUserService userService)
        {
            _validator = validator;
            _userService = userService;
        }

        /// <summary>
        /// Получение информации пользователем о себе
        /// </summary>
        /// <response code="200">Получение информации</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userService.GetUserInfo();

            return Ok(user);
        }

        /// <summary>
        /// Обновление своих данных пользователем
        /// </summary>
        /// <param name="userUpdate">Параметры обновления своих данных пользователем</param>
        /// <response code="200">Обновление информации</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("selfchange")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> UpdateUser([FromQuery] BaseUserUpdateParameters userUpdate)
        {
            await _userService.UpdateUser(userUpdate);

            return Ok();
        }

        /// <summary>
        /// Обновление пароля авторизованного пользователя
        /// </summary>
        /// <param name="newPassword">Новый пароль</param>
        /// <response code="200">Обновление пароля</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("change_pass")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> UpdateUserPassword([FromQuery] BaseUser newPassword)
        {
            await _validator.ValidateAndThrowAsync(newPassword);
            await _userService.UpdateUserPassword(newPassword);

            return Ok();
        }
    }
}
