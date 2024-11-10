using System.Text.Json.Serialization;

namespace LandAPI.API.Models
{
    public class DamageRelations
    {
        [JsonPropertyName("double_damage_from")]
        public string[] DoubleDamageFrom { get; set; }
        [JsonPropertyName("double_damage_to")]
        public string[] DoubleDamageTo { get; set; }
        [JsonPropertyName("half_damage_from")]
        public string[] HalfDamageFrom { get; set; }
        [JsonPropertyName("half_damage_to")]
        public string[] HalfDamageTo { get; set; }
        [JsonPropertyName("none_damage_from")]
        public string[] NoneDamageFrom { get; set; }
        [JsonPropertyName("none_damage_to")]
        public string[] NoneDamageTo { get; set; }
    }

    public class Type
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("damage_relations")]
        public DamageRelations DamageRelations { get; set; }
    }
}
