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
    public class ExpenseTypesController : ControllerBase
    {
        private readonly IExpenseTypeService _expenseTypeService;

        /// <summary>
        /// 
        /// </summary>
        public ExpenseTypesController(IExpenseTypeService expenseTypeService)
        {
            _expenseTypeService = expenseTypeService;
        }

        /// <summary>
        /// Получение списка всех категорий расхода
        /// </summary>
        /// /// <param name="sortBy">Параметры сортировки</param>
        /// <param name="pagination">Параметры пагинации</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(TypeSort sortBy, [FromQuery] Pagination pagination)
        {
            var expenseTypes = await _expenseTypeService.GetAll(pagination, sortBy);
            return Ok(expenseTypes);
        }

        /// <summary>
        /// Получение данных о категории расходов по идентификатору
        /// </summary>    
        /// <param name="id">Идентификатор категории расхода</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var expenseType = await _expenseTypeService.Get(id);

            return Ok(expenseType);
        }

        /// <summary>
        /// Сохранение категории расхода
        /// </summary>
        /// <param name="name">Наименование категории расхода</param>
        [HttpPost]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, string name)
        {
            await _expenseTypeService.Update(id, name);
            return Ok();
        }

        /// <summary>
        /// Удаление категории расходов по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор категории расхода</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _expenseTypeService.Delete(id);
            return Ok();
        }
    }
}
