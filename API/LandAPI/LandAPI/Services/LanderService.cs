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
        {
            return _landerRepository.GetAllLanders();
        }

        public Lander GetLanderById(int id)
        {
            return _landerRepository.GetLanderById(id);
        }

        public IEnumerable<Lander> GetLanderByType(string type)
        {
            return _landerRepository.GetLanderByType(type);
        }
    }
}
