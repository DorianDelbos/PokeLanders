namespace LandAPI.Models
{
    public class Stats
    {
        public int Base_Stat { get; set; }
        public string Stat { get; set; }
    }

    public class Lander
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Stats> Stats { get; set; }
        public int Base_Experience { get; set; }
        public int Base_Height { get; set; }
        public int Base_Weight { get; set; }
		public List<string> Types { get; set; }
	}
}