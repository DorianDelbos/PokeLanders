using System.Text.Json.Serialization;

namespace LandAPI.Models
{
	public class Moves
	{
		[JsonPropertyName("move")]
		public string Move { get; set; }
		[JsonPropertyName("move_learned_details")]
		public MoveLearnedDetails MoveLearnedDetails { get; set; }
	}

    public class Stats
	{
		[JsonPropertyName("base_stat")]
		public int BaseStat { get; set; }
		[JsonPropertyName("stat")]
		public string Stat { get; set; }
    }

    public class Lander
	{
		[JsonPropertyName("id")]
		public int ID { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("stats")]
		public List<Stats> Stats { get; set; }
		[JsonPropertyName("base_experience")]
		public int BaseExperience { get; set; }
		[JsonPropertyName("base_height")]
		public int BaseHeight { get; set; }
		[JsonPropertyName("base_weight")]
		public int BaseWeight { get; set; }
		[JsonPropertyName("types")]
		public List<string> Types { get; set; }
		[JsonPropertyName("moves")]
		public List<Moves> Moves { get; set; }
		[JsonPropertyName("model")]
		public string Model { get; set; }
	}
}