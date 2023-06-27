using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Options.Params.CreateUpdate;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;

namespace Project.Api.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IValidator<BaseUser> _validator;
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        public AdminUserController(IValidator<BaseUser> validator, IUserService userService)
        {
            _validator = validator;
            _userService = userService;
        }

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <param name="sortBy">Параметры сортировки</param>
        /// <param name="pagination">Параметры пагинации</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(UserSort sortBy, [FromQuery] Pagination pagination)
        {
            var users = await _userService.GetAll(sortBy, pagination);
            return Ok(users);
        }

        /// <summary>
        /// Получение данных о пользователе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.Get(id);

            return Ok(user);
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="userCreate">Параметры создания пользователя</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] AdminUserCreate userCreate)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] AdminUserUpdate userUpdate)
        {
            await _userService.Update(id, userUpdate);
            return Ok();
        }

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        /// <param name="password">Новый пароль</param>
        [HttpPut("{id}/change_user_pass")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromQuery] BaseUser password)
        {
            await _userService.UpdatePassword(id, password);
            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}
