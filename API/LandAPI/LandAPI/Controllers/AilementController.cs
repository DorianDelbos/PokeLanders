using LandAPI.Models;
using LandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class AilementController : ControllerBase
	{
		private readonly AilementService _ailementService;

		public AilementController(AilementService ailementService)
		{
			_ailementService = ailementService;
		}

		[HttpGet]
		public ActionResult<List<Ailement>> GetAilements()
		{
			var ailement = _ailementService.GetAllAilements();
			return Ok(ailement);
		}

		[HttpGet("{id}")]
		public ActionResult<Ailement> GetAilementById(int id)
		{
			var Lander = _ailementService.GetAilementById(id);
			if (Lander == null) return NotFound();
			return Ok(Lander);
		}
	}
}
