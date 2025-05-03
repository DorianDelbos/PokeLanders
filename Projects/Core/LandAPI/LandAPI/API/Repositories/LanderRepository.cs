using System.Text.Json;

namespace LandAPI.API
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
                _landers = JsonSerializer.Deserialize<List<Lander>>(jsonData)!;
            }

            _landers ??= new List<Lander>();
        }
    }
}
