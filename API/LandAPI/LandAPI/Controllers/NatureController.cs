using LandAPI.Models;
using LandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class NatureController : ControllerBase
	{
		private readonly NatureService _natureService;

		public NatureController(NatureService natureService)
		{
			_natureService = natureService;
		}

		[HttpGet]
		public ActionResult<List<Nature>> GetNatures()
		{
			var nature = _natureService.GetAllNatures();
			return Ok(nature);
		}

		[HttpGet("{id}")]
		public ActionResult<Nature> GetNatureById(int id)
		{
			var Lander = _natureService.GetNatureById(id);
			if (Lander == null) return NotFound();
			return Ok(Lander);
		}
	}
}
