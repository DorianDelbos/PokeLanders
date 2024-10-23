using LandAPI.Models;
using System.Text.Json;

namespace LandAPI.Data
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
				_evolutionChain = JsonSerializer.Deserialize<List<EvolutionChain>>(jsonData);
			}
			else
			{
				_evolutionChain = new List<EvolutionChain>();
			}
		}
    }
}
