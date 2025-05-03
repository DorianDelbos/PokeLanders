using System.Text.Json.Serialization;

namespace LandAPI.API
{
    public class EvolutionDetails
    {
        [JsonPropertyName("min_level")]
        public int? MinLevel { get; set; }
    }

    public class Chain
    {
        [JsonPropertyName("species")]
        public string Species { get; set; }
        [JsonPropertyName("evolution_details")]
        public List<EvolutionDetails> EvolutionDetails { get; set; }
        [JsonPropertyName("evolves_to")]
        public List<Chain> EvolvesTo { get; set; }
    }

    public class EvolutionChain
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("chain")]
        public Chain Chain { get; set; }
    }
}
