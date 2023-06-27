using Project.Core.Options.Params.Sort.Base;

namespace Project.Core.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> ReportGenerate(DateRange dataRange);
        Task ReportUpload();
    }
}
