using LandAPI.Models;
using LandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class AilmentController : ControllerBase
	{
		private readonly AilmentService _ailmentService;

		public AilmentController(AilmentService ailmentService)
		{
			_ailmentService = ailmentService;
		}

		[HttpGet]
		public ActionResult<List<Ailment>> GetAilements()
		{
			var ailement = _ailmentService.GetAllAilements();
			return Ok(ailement);
		}

		[HttpGet("{id}")]
		public ActionResult<Ailment> GetAilementById(int id)
		{
			var Lander = _ailmentService.GetAilementById(id);
			if (Lander == null) return NotFound();
			return Ok(Lander);
		}
	}
}
