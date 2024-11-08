using LandAPI.API.Data;
using LandAPI.API.Models;

namespace LandAPI.API.Services
{
    public class StatService
    {
        private readonly StatRepository _statRepository;

        public StatService(StatRepository statRepository)
        {
            _statRepository = statRepository;
        }

        public List<Stat> GetAllStats()
            => _statRepository.Stats;

        public Stat GetStatById(int id)
            => _statRepository.Stats.FirstOrDefault(p => p.ID == id);

        public IEnumerable<Stat> GetStatByName(string name)
            => _statRepository.Stats.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
