using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace LanderJsonCreator
{
    public class Stat
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class StatsManager
    {
        public ObservableCollection<Stat> StatsList { get; private set; } = new ObservableCollection<Stat>();

        public StatsManager(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = StatsList;
        }

        private void UpdateStats()
        {
            for (int i = 0; i < StatsList.Count; i++)
                StatsList[i].ID = i + 1;
        }

        public void RemoveStat(Stat statToRemove)
        {
            StatsList.Remove(statToRemove);
            UpdateStats();
        }

        public void AddStat(string name)
        {
            int newId = StatsList.Count > 0 ? StatsList.Max(s => s.ID) + 1 : 1;
            StatsList.Add(new Stat { ID = newId, Name = name });
        }
    }
}
