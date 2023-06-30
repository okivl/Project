using Microsoft.AspNetCore.Http;
using Project.Core.Models.SearchContexts;

namespace Project.Core.Interfaces
{
    /// <summary>
    /// Сервис отчета
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Получение отчета по расходам и доходам
        /// </summary>
        /// <param name="dataRange">Параметры фильтрации по дате</param>
        /// <returns>Отчет в Excel формате</returns>
        Task<byte[]> ReportGenerate(SearchContext dataRange);

        /// <summary>
        /// Загрузка отредактированного отчета
        /// </summary>
        Task ReportUpload(IFormFile file);
    }
}
