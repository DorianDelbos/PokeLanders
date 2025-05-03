using LandAPI.API.Data;
using LandAPI.API.Models;

namespace LandAPI.API.Services
{
    public class MoveService
    {
        private readonly MoveRepository _evolutionChainRepository;

        public MoveService(MoveRepository evolutionChainRepository)
        {
            _evolutionChainRepository = evolutionChainRepository;
        }

        public List<Move> GetAllMoves()
            => _evolutionChainRepository.Move;

        public Move GetMoveById(int id)
            => _evolutionChainRepository.Move.FirstOrDefault(p => p.ID == id);
    }
}
