using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Interfaces;
using Project.Core.Models.SearchContexts;

namespace Project.Api.Controllers
{
    /// <summary/>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary/>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Получение отчета по расходам и доходам
        /// </summary>
        /// <param name="dateRange">Параметры фильтрации по дате</param>
        [HttpGet("report")]
        public async Task<IActionResult> ReportGenerate([FromQuery] SearchContext dateRange)
        {
            var fileBytes = await _reportService.ReportGenerate(dateRange);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет.xlsx");
        }

        /// <summary>
        /// Загрузка отредактированного отчета
        /// </summary>
        [HttpPut("get_new_report")]
        public async Task<IActionResult> ReportUpload(IFormFile file)
        {
            await _reportService.ReportUpload(file);
            return Ok();
        }
    }
}
