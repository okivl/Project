using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Api.Controllers.Admin
{
    /// <summary/>
    [Authorize(Roles = "Admin")]
    [Route("api/admin/user")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IValidator<BaseUser> _validator;
        private readonly IUserService _userService;

        /// <summary/>
        public AdminUserController(IValidator<BaseUser> validator, IUserService userService)
        {
            _validator = validator;
            _userService = userService;
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <response code="200">Получение списка пользователей</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetAll([FromQuery] AdminUserSearchContext searchContext)
        {
            var users = await _userService.GetAll(searchContext);
            return Ok(users);
        }

        /// <summary>
        /// Получение данных о пользователе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <response code="200">Получение пользователя</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.Get(id);

            return Ok(user);
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="userCreate">Параметры создания пользователя</param>
        /// <response code="200">Создание пользователя</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create([FromQuery] AdminUserCreateParameters userCreate)
        {
            await _validator.ValidateAndThrowAsync(userCreate);
            await _userService.Create(userCreate);

            return Ok();
        }

        /// <summary>
        /// Обновление данных о пользователе
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        /// <param name="userUpdate">Параметры обновления данных о пользователе</param>
        /// <response code="200">Обновление пользователя</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(Guid id, [FromQuery] AdminUserUpdateParameters userUpdate)
        {
            await _userService.Update(id, userUpdate);
            return Ok();
        }

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        /// <param name="password">Новый пароль</param>
        /// <response code="200">Обновление пароля пользователя</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}/change_user_pass")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromQuery] BaseUser password)
        {
            await _userService.UpdatePassword(id, password);
            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        /// <response code="200">Удаление пользователя</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}
