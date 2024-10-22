namespace LandAPI.Models
{
	public class MoveAilement
	{
		public Ailement Ailement { get; set; }
		public int Chance { get; set; }
	}

	public class Move
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int Accuracy { get; set; }
		public int Power { get; set; }
		public int Pp { get; set; }
		public int Priority { get; set; }
		public Type Type { get; set; }
		public MoveAilement MoveAilement { get; set; }
	}
}
