using System.Text.Json.Serialization;

namespace LandAPI.Models
{
	public class DamageRelations
	{
		[JsonPropertyName("double_damage_from")]
		public String[] DoubleDamageFrom { get; set; }
		[JsonPropertyName("double_damage_to")]
		public String[] DoubleDamageTo { get; set; }
		[JsonPropertyName("half_damage_from")]
		public String[] HalfDamageFrom { get; set; }
		[JsonPropertyName("half_damage_to")]
		public String[] HalfDamageTo { get; set; }
		[JsonPropertyName("none_damage_from")]
		public String[] NoneDamageFrom { get; set; }
		[JsonPropertyName("none_damage_to")]
		public String[] NoneDamageTo { get; set; }
	}

	public class Type
	{
		[JsonPropertyName("id")]
		public int ID { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("damage_relations")]
		public DamageRelations DamageRelations { get; set; }
	}
}
