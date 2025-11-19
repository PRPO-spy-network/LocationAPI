using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers
{
	[ApiController]
	[Route("/kill")]
	public class KillController : Controller
	{
		private readonly IHostApplicationLifetime _lifetime;

		public KillController(IHostApplicationLifetime lifetime)
		{
			_lifetime = lifetime;
		}

		[HttpPut]
		public IActionResult Put()
		{
			Task.Run(() =>
			{
				Task.Delay(500).Wait();
				_lifetime.StopApplication();
			});

			return Ok("My only regret is not doing this sooner.");
		}
	}
}
