using LandAPI.API.Models;
using System.Text.Json;

namespace LandAPI.API.Data
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
                _move = JsonSerializer.Deserialize<List<Move>>(jsonData);
            }
            else
            {
                _move = new List<Move>();
            }
        }
    }
}
