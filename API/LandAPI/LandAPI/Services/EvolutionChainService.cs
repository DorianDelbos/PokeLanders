using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
	public class EvolutionChainService
    {
		private readonly EvolutionChainRepository _evolutionChainRepository;

		public EvolutionChainService(EvolutionChainRepository evolutionChainRepository)
		{
            _evolutionChainRepository = evolutionChainRepository;
		}

		public List<EvolutionChain> GetAllEvolutionChains()
		{
			return _evolutionChainRepository.GetAllEvolutionChain();
		}

		public EvolutionChain GetEvolutionChainById(int id)
		{
			return _evolutionChainRepository.GetEvolutionChainById(id);
		}
	}
}
