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
    public class AdminIncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        /// <summary>
        /// 
        /// </summary>
        public AdminIncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }


        /// <summary>
        /// Получение данных о всех доходах пользователей
        /// </summary>
        /// <param name="dateRange">Параметры фильтрации по дате</param>
        /// <param name="sortBy">Параметры сортировки</param>
        /// <param name="pagination">Параметры пагинации</param>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpGet("/all_user_incomes")]
        public async Task<IActionResult> AdminGetUserIncomes([FromQuery] DateRange dateRange, AdminIncomeExpenseSort sortBy, [FromQuery] Pagination pagination, Guid? id)
        {
            if (!id.HasValue) id = Guid.Empty;
            var incomes = await _incomeService.AdminGetUserIncomes(dateRange, sortBy, pagination, id);
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
        public async Task<IActionResult> Create([FromQuery] IncomeCU incomeCreate)
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
        public async Task<IActionResult> Update(Guid id, [FromQuery] IncomeCU incomeUpdate)
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
