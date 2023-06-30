using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Authorize]
    [Route("api/[controller]")]
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
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] IncomeExpenseSearchContext searchContext)
        {
            var expense = await _expenseService.GetAll(searchContext);
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
        public async Task<IActionResult> Create([FromQuery] ExpenseCreateUpdateParameters expenseCreate)
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
        public async Task<IActionResult> Update(Guid id, [FromQuery] ExpenseCreateUpdateParameters expenseCreate)
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
