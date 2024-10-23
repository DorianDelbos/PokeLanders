using LandAPI.Models;
using System.Text.Json;

namespace LandAPI.Data
{
	public class StatRepository
	{
		private readonly string _filePath = "wwwroot/Assets/Data/stats.json";
		private List<Stat> _stats;

		public List<Stat> Stats => _stats;

		public StatRepository()
		{
			if (File.Exists(_filePath))
			{
				string jsonData = File.ReadAllText(_filePath);
				_stats = JsonSerializer.Deserialize<List<Stat>>(jsonData);
			}
			else
			{
				_stats = new List<Stat>();
			}
		}
	}
}
