﻿using LandAPI.API.Data;
using LandAPI.API.Models;

namespace LandAPI.API.Services
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

        public Lander GetLanderByName(string name)
            => _landerRepository.Landers.FirstOrDefault(p => p.Name == name);

        public IEnumerable<Lander> GetLanderByType(string type)
            => _landerRepository.Landers.Where(p => p.Types.Any(t => t.Equals(type, StringComparison.OrdinalIgnoreCase)));
    }
}
