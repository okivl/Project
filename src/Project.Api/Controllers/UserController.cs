using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
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
        [HttpGet("me")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userService.GetUserInfo();

            return Ok(user);
        }

        /// <summary>
        /// Обновление своих данных пользователем
        /// </summary>
        /// <param name="userUpdate">Параметры обновления своих данных пользователем</param>
        [HttpPut("selfchange")]
        public async Task<IActionResult> UpdateUser([FromQuery] BaseUserUpdateParameters userUpdate)
        {
            await _userService.UpdateUser(userUpdate);

            return Ok();
        }

        /// <summary>
        /// Обновление пароля авторизованного пользователя
        /// </summary>
        /// <param name="newPassword">Новый пароль</param>
        [HttpPut("change_pass")]
        public async Task<IActionResult> UpdateUserPassword([FromQuery] BaseUser newPassword)
        {
            await _validator.ValidateAndThrowAsync(newPassword);
            await _userService.UpdateUserPassword(newPassword);

            return Ok();
        }
    }
}
