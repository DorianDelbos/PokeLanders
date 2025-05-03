using LandAPI.API.Models;
using LandAPI.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LandAPI.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly MoveService _moveService;

        public MoveController(MoveService moveService)
        {
            _moveService = moveService;
        }

        [HttpGet]
        public ActionResult<List<Move>> GetMoves()
        {
            var move = _moveService.GetAllMoves();
            return Ok(move);
        }

        [HttpGet("{id}")]
        public ActionResult<Move> GetMoveById(int id)
        {
            var Lander = _moveService.GetMoveById(id);
            if (Lander == null) return NotFound();
            return Ok(Lander);
        }
    }
}
