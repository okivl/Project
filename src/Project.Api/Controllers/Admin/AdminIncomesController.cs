using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models.CreateUpdate;
using Project.Core.Models.SearchContexts;

namespace Project.Api.Controllers.Admin
{
    /// <summary/>
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminIncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        /// <summary/>
        public AdminIncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }


        /// <summary>
        /// Получение данных о всех доходах пользователей
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        [HttpGet("/all_user_incomes")]
        public async Task<IActionResult> AdminGetUserIncomes([FromQuery] AdminIncomeExpenseSearchContext searchContext)
        {
            if (!searchContext.Id.HasValue) searchContext.Id = Guid.Empty;
            var incomes = await _incomeService.AdminGetUserIncomes(searchContext);
            return Ok(incomes);
        }


        /// <summary>
        /// Получение данных о доходе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var income = await _incomeService.Get(id);

            return Ok(income);
        }

        /// <summary>
        /// Сохранение дохода
        /// </summary>
        /// <param name="incomeCreate">Параметры создания дохода</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] IncomeCraeteUpdateParameters incomeCreate)
        {
            await _incomeService.Create(incomeCreate);

            return Ok();
        }

        /// <summary>
        /// Редактирование дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        /// <param name="incomeUpdate">Параметры обновления дохода</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] IncomeCraeteUpdateParameters incomeUpdate)
        {
            await _incomeService.Update(id, incomeUpdate);
            return Ok();
        }

        /// <summary>
        /// Удаление источника дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _incomeService.Delete(id);
            return Ok();
        }
    }
}
