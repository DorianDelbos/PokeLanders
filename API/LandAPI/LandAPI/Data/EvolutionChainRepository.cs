using LandAPI.Models;

namespace LandAPI.Data
{
    public class EvolutionChainRepository
    {
        private List<EvolutionChain> _evolutionChain;

        public EvolutionChainRepository()
        {
            InitializeEvolutionChain();
        }

        private void InitializeEvolutionChain()
        {
            _evolutionChain = new List<EvolutionChain>
            {

            };
        }

        public List<EvolutionChain> GetAllEvolutionChain() => _evolutionChain;

        public EvolutionChain GetEvolutionChainById(int id) => _evolutionChain.FirstOrDefault(p => p.ID == id);
    }
}
