using LandAPI.Models;
using System.Text.Json;

namespace LandAPI.Data
{
	public class LanderRepository
	{
		private readonly string _filePath = "wwwroot/Assets/Data/landers.json";
		private List<Lander> _landers;

		public List<Lander> Landers => _landers;

		public LanderRepository()
		{
			if (File.Exists(_filePath))
			{
				string jsonData = File.ReadAllText(_filePath);
				_landers = JsonSerializer.Deserialize<List<Lander>>(jsonData);
			}
			else
			{
				_landers = new List<Lander>();
			}
		}
	}
}
