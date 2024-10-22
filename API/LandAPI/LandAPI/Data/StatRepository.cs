using LandAPI.Models;

namespace LandAPI.Data
{
	public class StatRepository
	{
		private List<Stat> _stats;

		public StatRepository()
		{
			InitializeStat();
		}

		private void InitializeStat()
		{
			_stats = new List<Stat>
			{
				new Stat
				{
					ID = 1,
					Name = "pv"
				},
				new Stat
				{
					ID = 2,
					Name = "attack"
				},
				new Stat
				{
					ID = 3,
					Name = "defense"
				},
				new Stat
				{
					ID = 4,
					Name = "special-attack"
				},
				new Stat
				{
					ID = 5,
					Name = "special-defense"
				},
				new Stat
				{
					ID = 6,
					Name = "speed"
				}
			};
		}

		public List<Stat> GetAllStats() => _stats;

		public Stat GetStatById(int id) => _stats.FirstOrDefault(p => p.ID == id);

		public IEnumerable<Stat> GetStatByName(string name)
		{
			return _stats.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}
	}
}
