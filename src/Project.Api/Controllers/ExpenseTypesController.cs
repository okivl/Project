using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Authorize]
    [Route("api/expense_types")]
    [ApiController]
    public class ExpenseTypesController : ControllerBase
    {
        private readonly IExpenseTypeService _expenseTypeService;

        /// <summary/>
        public ExpenseTypesController(IExpenseTypeService expenseTypeService)
        {
            _expenseTypeService = expenseTypeService;
        }

        /// <summary>
        /// Получение списка всех категорий расхода
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <response code="200">Получение списка категорий</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ExpenseType>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetAll([FromQuery] IncomeExpenseTypeSearchContext searchContext)
        {
            var expenseTypes = await _expenseTypeService.GetAll(searchContext);
            return Ok(expenseTypes);
        }

        /// <summary>
        /// Получение данных о категории расходов по идентификатору
        /// </summary>    
        /// <param name="id">Идентификатор категории расхода</param>
        /// <response code="200">Получение категории</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseType), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Get(Guid id)
        {
            var expenseType = await _expenseTypeService.Get(id);

            return Ok(expenseType);
        }

        /// <summary>
        /// Сохранение категории расхода
        /// </summary>
        /// <param name="name">Наименование категории расхода</param>
        /// <response code="200">Создание категории</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create(string name)
        {
            await _expenseTypeService.Create(name);

            return Ok();
        }

        /// <summary>
        /// Редактирование категории расходов по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории расхода</param>
        /// <param name="name">Наименование категории расхода</param>
        /// <response code="200">Обновление категории</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(Guid id, string name)
        {
            await _expenseTypeService.Update(id, name);
            return Ok();
        }

        /// <summary>
        /// Удаление категории расходов по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории расхода</param>
        /// <response code="200">Удаление категории</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _expenseTypeService.Delete(id);
            return Ok();
        }
    }
}
