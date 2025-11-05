using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Locations.Models;


namespace Locations.Controllers
{
	[ApiController]
	[Route("/car")]
	public class LocationController : ControllerBase
	{
		private readonly ILogger<LocationController> _logger;
		private readonly IDbContextFactory<PostgresContext> _dbContextFactory;
		public LocationController(ILogger<LocationController> logger, IDbContextFactory<PostgresContext> dbContextFactory)
		{
			_logger = logger;
			_dbContextFactory = dbContextFactory;
		}

		[HttpGet]
		public IActionResult Get([FromQuery] int? timeframe)
		{
			using (var dbContext = _dbContextFactory.CreateDbContext())
			{
				var positions = from pos in dbContext.CarGpsData select pos;

				if (timeframe.HasValue)
				{
					var cuttoff = DateTime.UtcNow.AddDays(-timeframe.Value);
					positions = positions.Where(pos => pos.Time >= cuttoff);
				}

				var resultList = positions.ToList();

				if (!resultList.Any())
				{
					return NotFound(new { message = "No GPS data found for the specified criteria." });
				}

				return Ok(resultList);
			}
		}

		[HttpGet("/car/{carId}")]
		public IActionResult Get(string? carId, [FromQuery] int? timeframe)
		{
			using (var dbContext = _dbContextFactory.CreateDbContext())
			{
				// Lazy
				var positions = from pos in dbContext.CarGpsData where pos.CarId == carId select pos;

				if (timeframe.HasValue)
				{
					var cuttoff = DateTime.UtcNow.AddDays(-timeframe.Value);
					positions = positions.Where(pos => pos.Time >= cuttoff);
				}

				var resultList = positions.ToList();

				if (!resultList.Any())
				{
					return NotFound(new { message = "No GPS data found for the specified criteria." });
				}

				return Ok(resultList);
			}
		}
	}
}

