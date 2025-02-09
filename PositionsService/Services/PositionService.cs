using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using PositionsService.Data;
using PositionsService.Models;

namespace PositionsService.Services
{
    public class PositionService : IPositionService
    {
        private readonly PositionsDbContext _dbContext;
        private readonly ILogger<PositionService> _logger;

        public PositionService(PositionsDbContext dbContext, ILogger<PositionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UpdatePositionPricesAsync(string instrumentId, decimal newRate)
        {
            var positions = await _dbContext.FinancialPositions
                .Where(p => p.InstrumentId == instrumentId)
                .ToListAsync();

            foreach (var position in positions)
            {
                decimal profitLoss = position.Quantity * (newRate - position.InitialRate) * (int)position.Side;
                _logger.LogInformation($"Position {position.InstrumentId}: P/L = {profitLoss} USD");
            }
        }

        public async Task ImportPositionsFromCsvAsync(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var positions = csv.GetRecords<FinancialPosition>().ToList();

            await _dbContext.FinancialPositions.AddRangeAsync(positions);
            await _dbContext.SaveChangesAsync();
        }
    }
}
