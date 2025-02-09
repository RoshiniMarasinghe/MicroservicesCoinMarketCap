using System.Threading.Tasks;

namespace PositionsService.Services
{
    public interface IPositionService
    {
        Task UpdatePositionPricesAsync(string instrumentId, decimal newRate);
        Task ImportPositionsFromCsvAsync(string filePath);
    }
}
