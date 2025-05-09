﻿namespace LandAPI.API
{
    public class EvolutionChainService
    {
        private readonly EvolutionChainRepository _evolutionChainRepository;

        public EvolutionChainService(EvolutionChainRepository evolutionChainRepository)
        {
            _evolutionChainRepository = evolutionChainRepository;
        }

        public List<EvolutionChain> GetAllEvolutionChains()
            => _evolutionChainRepository.EvolutionChain;

        public EvolutionChain? GetEvolutionChainById(int id)
            => _evolutionChainRepository.EvolutionChain.FirstOrDefault(p => p.ID == id);
    }
}
