using System.Text.Json;

namespace LandAPI.API
{
    public class TypeRepository
    {
        private readonly string _filePath = "wwwroot/Assets/Data/types.json";
        private List<Type> _types;

        public List<Type> Types => _types;

        public TypeRepository()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                _types = JsonSerializer.Deserialize<List<Type>>(jsonData)!;
            }

            _types ??= new List<Type>();
        }
    }
}
