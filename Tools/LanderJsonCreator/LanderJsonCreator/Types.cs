using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace LanderJsonCreator
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
        [JsonPropertyName("damage_relations")]
        public DamageRelations DamageRelations { get; set; } = new DamageRelations();
    }

    public class TypesManager
    {
        public ObservableCollection<Type> TypesList { get; private set; } = new ObservableCollection<Type>();

        public TypesManager(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = TypesList;
        }

        private void UpdateStats()
        {
            for (int i = 0; i < TypesList.Count; i++)
                TypesList[i].ID = i + 1;
        }

        public void RemoveStat(Type statToRemove)
        {
            TypesList.Remove(statToRemove);
            UpdateStats();
        }

        public void AddStat(string name)
        {
            int newId = TypesList.Count > 0 ? TypesList.Max(s => s.ID) + 1 : 1;
            TypesList.Add(new Type { ID = newId, Name = name });
        }
    }
}
