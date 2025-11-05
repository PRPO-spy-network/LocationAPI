using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Locations.Classes;
using Locations.Models;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Locations.Controllers
{
    [ApiController]
	[Route("/location/[controller]")]
	public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
		private readonly IDbContextFactory<PostgresContext> _dbContextFactory;
		public LocationController(ILogger<LocationController> logger, IDbContextFactory<PostgresContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			using (var dbContext = _dbContextFactory.CreateDbContext())
			{
				var registration = (from r in dbContext.Registrations
									from l in dbContext.LookupRegistrations
									where r.RegionId == l.Id && r.CarId == id
									select new {Id = r.CarId, Region = l.Region})
					.FirstOrDefault();

				if (registration == null)
				{
					return NotFound(new { message = "Registration not found!" });
				}

				return Ok(registration);
			}
		}

		[HttpGet]
		public IActionResult Get()
		{
			using (var dbContext = _dbContextFactory.CreateDbContext())
			{
				var positions = from pos in dbContext.CarGpsData select pos;

				if (positions == null)
				{
					return NotFound(new { message = "Registrations not found!" });
				}

				return Ok(positions.ToList());
			}
		}
	}
}

