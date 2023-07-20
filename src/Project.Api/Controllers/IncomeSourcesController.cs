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
    [Route("api/income_sources")]
    [ApiController]
    public class IncomeSourcesController : ControllerBase
    {
        private readonly IIncomeSourceService _incomeSourcesService;

        /// <summary/>
        public IncomeSourcesController(IIncomeSourceService incomeSourceService)
        {
            _incomeSourcesService = incomeSourceService;
        }

        /// <summary>
        /// Получение списка всех источников дохода
        /// </summary>
        /// <param name="searchContext">Параметры поиска</param>
        /// /// <response code="200">Получение списка источников</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<IncomeSource>), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> GetAll([FromQuery] IncomeExpenseTypeSearchContext searchContext)
        {
            var incomeSources = await _incomeSourcesService.GetAll(searchContext);
            return Ok(incomeSources);
        }

        /// <summary>
        /// Получение данных об источнике дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор источника дохода</param>
        /// <response code="200">Получение источника</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IncomeSource), 200)]
        [ProducesResponseType(typeof(ExceptionResponse), 400)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Get(Guid id)
        {
            var incomeSource = await _incomeSourcesService.Get(id);

            return Ok(incomeSource);
        }

        /// <summary>
        /// Сохранение источника дохода
        /// </summary>  
        /// <param name="name">Наименование источника дохода</param>
        /// <response code="200">Создание источника</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Create(string name)
        {
            await _incomeSourcesService.Create(name);

            return Ok();
        }

        /// <summary>
        /// Редактирование источника дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор источника дохода</param>
        /// <param name="name">Наименование источника дохода</param>
        /// <response code="200">Обновление источника</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Update(Guid id, string name)
        {
            await _incomeSourcesService.Update(id, name);
            return Ok();
        }

        /// <summary>
        /// Удаление источника дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор источника дохода</param>
        /// <response code="200">Удаление источника</response>
        /// <response code="404">Не найдено</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ExceptionResponse), 404)]
        [ProducesResponseType(typeof(ExceptionResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _incomeSourcesService.Delete(id);

            return Ok();
        }
    }
}
