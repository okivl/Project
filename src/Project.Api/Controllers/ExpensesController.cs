using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;
using Project.Entities;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Authorize]
    [Route("api/expense")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        /// <summary/>
        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Получение списка всех расходов за период
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// <response code="200">Получение списка расходов</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Expense>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetUserExpenses([FromQuery] IncomeExpenseSearchContext searchContext)
        {
            var expense = await _expenseService.GetUserExpenses(searchContext);
            return Ok(expense);
        }

        /// <summary>
        /// Получение данных о расходе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <response code="200">Получение расхода</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Expense), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Get(Guid id)
        {
            var expense = await _expenseService.Get(id);

            return Ok(expense);
        }

        /// <summary>
        /// Сохранение расхода
        /// </summary>
        /// <param name="expenseCreate">Параметры создания расхода</param>
        /// <response code="200">Создание расхода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create([FromQuery] ExpenseCreateParameters expenseCreate)
        {
            await _expenseService.Create(expenseCreate);

            return Ok();
        }

        /// <summary>
        /// Редактирование расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <param name="expenseCreate">Параметры обновления расхода</param>
        /// <response code="200">Обновление расхода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(Guid id, [FromQuery] ExpenseUpdateParameters expenseCreate)
        {
            await _expenseService.Update(id, expenseCreate);
            return Ok();
        }

        /// <summary>
        /// Удаление расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <response code="200">Удаление расхода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _expenseService.Delete(id);
            return Ok();
        }
    }
}
