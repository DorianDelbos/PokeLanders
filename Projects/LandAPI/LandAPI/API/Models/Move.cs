using System.Text.Json.Serialization;

namespace LandAPI.API.Models
{
    public class MoveLearnedDetails
    {
        [JsonPropertyName("level_learned_at")]
        public int LevelLearnedAt { get; set; }
    }

    public class MoveAilement
    {
        [JsonPropertyName("ailement")]
        public string Ailement { get; set; }
        [JsonPropertyName("chance")]
        public int Chance { get; set; }
    }

    public class Move
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("accuracy")]
        public int Accuracy { get; set; }
        [JsonPropertyName("power")]
        public int Power { get; set; }
        [JsonPropertyName("pp")]
        public int Pp { get; set; }
        [JsonPropertyName("priority")]
        public int Priority { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("move_ailement")]
        public MoveAilement MoveAilement { get; set; }
    }
}
