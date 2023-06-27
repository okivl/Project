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
    public class AdminExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        /// <summary>
        /// 
        /// </summary>
        public AdminExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Получение данных о всех расходах пользователей
        /// </summary>
        /// <param name="dateRange">Параметры фильтрации по дате</param>
        /// <param name="sortBy">Параметры сортировки</param>
        /// <param name="pagination">Параметры пагинации</param>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpGet("/all_expenses")]
        public async Task<IActionResult> AdminGetUserExpenses([FromQuery] DateRange dateRange, AdminIncomeExpenseSort sortBy, [FromQuery] Pagination pagination, Guid? id)
        {
            if (!id.HasValue) id = Guid.Empty;
            var expense = await _expenseService.AdminGetUserExpenses(dateRange, sortBy, pagination, id);
            return Ok(expense);
        }

        /// <summary>
        /// Получение данных о расходе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var expense = await _expenseService.Get(id);

            return Ok(expense);
        }

        /// <summary>
        /// Сохранение расхода
        /// </summary>
        /// <param name="expenseCreate">Параметры создания расхода</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] ExpenseCU expenseCreate)
        {
            await _expenseService.Create(expenseCreate);

            return Ok();
        }

        /// <summary>
        /// Редактирование расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        /// <param name="expenseCreate">Параметры обновления расхода</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] ExpenseCU expenseCreate)
        {
            await _expenseService.Update(id, expenseCreate);
            return Ok();
        }

        /// <summary>
        /// Удаление расхода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор расхода</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _expenseService.Delete(id);
            return Ok();
        }
    }
}
