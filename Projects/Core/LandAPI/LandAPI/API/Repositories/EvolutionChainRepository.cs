using System.Text.Json;

namespace LandAPI.API
{
    public class EvolutionChainRepository
    {
        private readonly string _filePath = "wwwroot/Assets/Data/evolutionChains.json";
        private List<EvolutionChain> _evolutionChain;

        public List<EvolutionChain> EvolutionChain => _evolutionChain;

        public EvolutionChainRepository()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                _evolutionChain = JsonSerializer.Deserialize<List<EvolutionChain>>(jsonData)!;
            }

            _evolutionChain ??= new List<EvolutionChain>();
        }
    }
}
