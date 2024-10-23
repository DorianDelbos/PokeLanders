using LandAPI.Models;
using System.Text.Json;

namespace LandAPI.Data
{
    public class TypeRepository
    {
		private readonly string _filePath = "wwwroot/Assets/Data/types.json";
        private List<Models.Type> _types;

		public List<Models.Type> Types => _types;

		public TypeRepository()
		{
			if (File.Exists(_filePath))
			{
				string jsonData = File.ReadAllText(_filePath);
				_types = JsonSerializer.Deserialize<List<Models.Type>>(jsonData);
			}
			else
			{
				_types = new List<Models.Type>();
			}
		}
    }
}
