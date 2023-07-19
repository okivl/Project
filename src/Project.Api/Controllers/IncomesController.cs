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
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        /// <summary/>
        public IncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        /// <summary>
        /// Получение списка всех доходов за период
        /// </summary> 
        /// <param name="searchContext">Параметры поиска</param>
        /// <response code="200">Получение списка доходов</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Income>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetUserIncomes([FromQuery] IncomeExpenseSearchContext searchContext)
        {
            var incomes = await _incomeService.GetUserIncomes(searchContext);
            return Ok(incomes);
        }

        /// <summary>
        /// Получение данных о доходе по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        /// <response code="200">Получение дохода</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Income), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Get(Guid id)
        {
            var income = await _incomeService.Get(id);

            return Ok(income);
        }

        /// <summary>
        /// Сохранение дохода
        /// </summary>
        /// <param name="incomeCreate">Параметры создания дохода</param>
        /// <response code="200">Создание дохода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create([FromQuery] IncomeCreateParameters incomeCreate)
        {
            await _incomeService.Create(incomeCreate);

            return Ok();
        }

        /// <summary>
        /// Редактирование дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        /// <param name="incomeUpdate">Параметры обновления дохода</param>
        /// <response code="200">Обновление дохода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(Guid id, [FromQuery] IncomeUpdateParameters incomeUpdate)
        {
            await _incomeService.Update(id, incomeUpdate);
            return Ok();
        }

        /// <summary>
        /// Удаление источника дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор дохода</param>
        /// <response code="200">Удаление расхода</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _incomeService.Delete(id);
            return Ok();
        }
    }
}
