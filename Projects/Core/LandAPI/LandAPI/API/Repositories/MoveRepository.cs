using System.Text.Json;

namespace LandAPI.API
{
    public class MoveRepository
    {
        private readonly string _filePath = "wwwroot/Assets/Data/moves.json";
        private List<Move> _move;

        public List<Move> Move => _move;

        public MoveRepository()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                _move = JsonSerializer.Deserialize<List<Move>>(jsonData)!;
            }

            _move ??= new List<Move>();
        }
    }
}
