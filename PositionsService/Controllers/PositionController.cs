using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using PositionsService.Services;

namespace PositionsService.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private IPositionService _positionsService; 

        public PositionController(IPositionService positionsService)
        {
            _positionsService = positionsService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumentId"></param>
        /// <param name="newRate"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> GetAllPositions(string instrumentId, decimal newRate)
        {
            await _positionsService.UpdatePositionPricesAsync(instrumentId, newRate);
            return Ok();
        }
    }
}
