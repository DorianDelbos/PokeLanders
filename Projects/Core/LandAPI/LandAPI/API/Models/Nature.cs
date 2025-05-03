using System.Text.Json.Serialization;

namespace LandAPI.API.Models
{
    public class StatChanges
    {
        [JsonPropertyName("change")]
        public int change { get; set; }
        [JsonPropertyName("stat")]
        public string Stat { get; set; }
    }

    public class Nature
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("stat_changes")]
        public StatChanges[] StatChanges { get; set; }
    }
}
