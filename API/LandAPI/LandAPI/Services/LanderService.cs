using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
    public class LanderService
    {
        private readonly LanderRepository _landerRepository;

        public LanderService(LanderRepository LanderRepository)
        {
            _landerRepository = LanderRepository;
        }

        public List<Lander> GetAllLanders()
            => _landerRepository.Landers;

        public Lander GetLanderById(int id)
            => _landerRepository.Landers.FirstOrDefault(p => p.ID == id);


		public IEnumerable<Lander> GetLanderByType(string type) 
            => _landerRepository.Landers.Where(p => p.Types.Any(t => t.Equals(type, StringComparison.OrdinalIgnoreCase)));
	}
}
