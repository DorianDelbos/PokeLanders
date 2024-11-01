using Microsoft.AspNetCore.Mvc;
using LandAPI.API.Models;
using LandAPI.API.Services;

namespace LandAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LanderController : ControllerBase
    {
        private readonly LanderService _landerService;

        public LanderController(LanderService LanderService)
        {
            _landerService = LanderService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Lander>> GetAllLanders()
        {
            return Ok(_landerService.GetAllLanders());
        }

        [HttpGet("{id}")]
        public ActionResult<Lander> GetLanderById(int id)
        {
            var Lander = _landerService.GetLanderById(id);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }

        [HttpGet("name/{name}")]
        public ActionResult<Lander> GetLanderByName(string name)
        {
            var Lander = _landerService.GetLanderByName(name);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }

        [HttpGet("type/{type}")]
        public ActionResult<IEnumerable<Lander>> GetLanderByType(string type)
        {
            return Ok(_landerService.GetLanderByType(type));
        }
    }
}
