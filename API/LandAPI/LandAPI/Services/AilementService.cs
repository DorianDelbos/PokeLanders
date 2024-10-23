using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
	public class AilementService
	{
		private readonly AilementRepository _evolutionChainRepository;

		public AilementService(AilementRepository evolutionChainRepository)
		{
			_evolutionChainRepository = evolutionChainRepository;
		}

		public List<Ailement> GetAllAilements()
			=> _evolutionChainRepository.Ailement;

		public Ailement GetAilementById(int id)
			=> _evolutionChainRepository.Ailement.FirstOrDefault(p => p.ID == id);
	}
}
