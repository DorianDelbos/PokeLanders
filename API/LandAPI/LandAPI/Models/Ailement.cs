using System.Text.Json.Serialization;

namespace LandAPI.Models
{
	public class Ailement
	{
		[JsonPropertyName("id")]
		public int ID { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
