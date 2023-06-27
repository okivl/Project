using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Options.Params.Sort;
using Project.Core.Options.Params.Sort.Base;

namespace Project.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeSourcesController : ControllerBase
    {
        private readonly IIncomeSourceService _incomeSourcesService;

        /// <summary>
        /// 
        /// </summary>
        public IncomeSourcesController(IIncomeSourceService incomeSourceService)
        {
            _incomeSourcesService = incomeSourceService;
        }

        /// <summary>
        /// Получение списка всех источников дохода
        /// </summary>
        /// <param name="sortBy">Параметры сортировки</param>
        /// <param name="pagination">Параметры пагинации</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(TypeSort sortBy, [FromQuery] Pagination pagination)
        {
            var incomeSources = await _incomeSourcesService.GetAll(pagination, sortBy);
            return Ok(incomeSources);
        }

        /// <summary>
        /// Получение данных об источнике дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор источника дохода</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var incomeSource = await _incomeSourcesService.Get(id);

            return Ok(incomeSource);
        }

        /// <summary>
        /// Сохранение источника дохода
        /// </summary>  
        /// <param name="name">Наименование источника дохода</param>
        [HttpPost]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, string name)
        {
            await _incomeSourcesService.Update(id, name);
            return Ok();
        }

        /// <summary>
        /// Удаление источника дохода по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор источника дохода</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _incomeSourcesService.Delete(id);

            return Ok();
        }
    }
}
