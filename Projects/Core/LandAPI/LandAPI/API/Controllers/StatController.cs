using LandAPI.API.Models;
using LandAPI.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        private readonly StatService _StatService;

        public StatController(StatService StatService)
        {
            _StatService = StatService;
        }

        [HttpGet]
        public ActionResult<List<Stat>> GetStats()
        {
            var Stats = _StatService.GetAllStats();
            return Ok(Stats);
        }

        [HttpGet("{id}")]
        public ActionResult<Stat> GetStatById(int id)
        {
            var Lander = _StatService.GetStatById(id);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IEnumerable<Stat>> GetStatByStat(string name)
        {
            return Ok(_StatService.GetStatByName(name));
        }
    }
}
