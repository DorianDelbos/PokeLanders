using LandAPI.Models;
using LandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly TypeService _typeService;

        public TypeController(TypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public ActionResult<List<Models.Type>> GetTypes()
        {
            var types = _typeService.GetAllTypes();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public ActionResult<Lander> GetLanderById(int id)
        {
            var Lander = _typeService.GetLanderById(id);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }

        [HttpGet("name/{name}")]
        public ActionResult<IEnumerable<Models.Type>> GetLanderByType(string name)
        {
            return Ok(_typeService.GetTypeByName(name));
        }
    }
}
