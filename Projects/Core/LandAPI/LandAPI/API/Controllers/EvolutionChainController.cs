using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EvolutionChainController : ControllerBase
    {
        private readonly EvolutionChainService _evolutionChainService;

        public EvolutionChainController(EvolutionChainService evolutionChainService)
        {
            _evolutionChainService = evolutionChainService;
        }

        [HttpGet]
        public ActionResult<List<EvolutionChain>> GetEvolutionChains()
        {
            var evolutionChain = _evolutionChainService.GetAllEvolutionChains();
            return Ok(evolutionChain);
        }

        [HttpGet("{id}")]
        public ActionResult<EvolutionChain> GetEvolutionChainById(int id)
        {
            var Lander = _evolutionChainService.GetEvolutionChainById(id);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }
    }
}
