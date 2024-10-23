using LandAPI.Models;
using System.Text.Json;

namespace LandAPI.Data
{
	public class AilementRepository
	{
		private readonly string _filePath = "wwwroot/Assets/Data/ailements.json";
		private List<Ailement> _ailement;

		public List<Ailement> Ailement => _ailement;

		public AilementRepository()
		{
			if (File.Exists(_filePath))
			{
				string jsonData = File.ReadAllText(_filePath);
				_ailement = JsonSerializer.Deserialize<List<Ailement>>(jsonData);
			}
			else
			{
				_ailement = new List<Ailement>();
			}
		}
	}
}
