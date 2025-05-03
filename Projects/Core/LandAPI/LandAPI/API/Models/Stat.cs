using System.Text.Json.Serialization;

namespace LandAPI.API
{
    public class Stat
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
