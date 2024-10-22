using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
	public class StatService
	{
		private readonly StatRepository _statRepository;

		public StatService(StatRepository statRepository)
		{
			_statRepository = statRepository;
		}

		public List<Stat> GetAllStats()
		{
			return _statRepository.GetAllStats();
		}

		public Stat GetStatById(int id)
		{
			return _statRepository.GetStatById(id);
		}

		public IEnumerable<Stat> GetStatByName(string name)
		{
			return _statRepository.GetStatByName(name);
		}
	}
}
