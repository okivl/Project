using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Core.Exeptions;
using Project.Core.Extensions;
using Project.Core.Interfaces;
using Project.Core.Models.SearchContexts;
using Project.Infrastructure.Data;

namespace Project.Core.Services
{
    /// <inheritdoc cref="IReportService"/>
    public class ReportService : IReportService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public ReportService(IUserService userService, DataContext context)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<byte[]> ReportGenerate(SearchContext dataRange)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Отчет");

            worksheet.AddTableHeaders();

            var user = await _userService.GetUserInfo();

            var incomes = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                .Where(i => i.CreatedAt >= dataRange.DateFrom && i.CreatedAt <= dataRange.DateTo && i.User.Id == user.Id).ToListAsync();

            var currentRow = 1;
            foreach (var income in incomes)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = income.Id.ToString();
                worksheet.Cell(currentRow, 2).Value = income.Name;
                worksheet.Cell(currentRow, 3).Value = income.Amount;
            }


            var expenses = await _context.Expenses.Include(i => i.ExpenseType)
                .Where(i => i.CreatedAt >= dataRange.DateFrom && i.CreatedAt <= dataRange.DateTo && i.User.Id == user.Id).ToListAsync();

            currentRow = 1;
            foreach (var expense in expenses)
            {
                currentRow++;
                worksheet.Cell(currentRow, 4).Value = expense.Id.ToString();
                worksheet.Cell(currentRow, 5).Value = expense.Name;
                worksheet.Cell(currentRow, 6).Value = expense.Amount;
            }
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }

        public async Task ReportUpload(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet("Отчет");

            int numberOfLastRow = worksheet.Column(1).LastCellUsed().WorksheetRow().RowNumber();

            for (int i = 2; i <= numberOfLastRow; i++)
            {
                var incomeId = new Guid(worksheet.Cell(i, 1).Value.ToString());

                var income = await _context.Incomes.Include(i => i.IncomeSource).Include(u => u.User)
                    .SingleOrDefaultAsync(j => j.Id == incomeId)
                    ?? throw new NotFoundException();

                income.Name = worksheet.Cell(i, 2).Value.ToString();

                income.Amount = decimal.Parse(worksheet.Cell(i, 3).Value.ToString());

                await _context.SaveChangesAsync();
            }
            numberOfLastRow = worksheet.Column(4).LastCellUsed().WorksheetRow().RowNumber();
            for (int i = 2; i <= numberOfLastRow; i++)
            {
                var expenseId = new Guid(worksheet.Cell(i, 4).Value.ToString());

                var expense = await _context.Expenses.Include(i => i.ExpenseType).Include(u => u.User)
                    .SingleOrDefaultAsync(j => j.Id == expenseId)
                    ?? throw new NotFoundException();

                expense.Name = worksheet.Cell(i, 5).Value.ToString();

                expense.Amount = decimal.Parse(worksheet.Cell(i, 6).Value.ToString());

                await _context.SaveChangesAsync();
            }
        }
    }
}
