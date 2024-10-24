using System.Collections.Generic;

namespace Lander.Extern.API
{
	[System.Serializable]
	public class Moves
	{
		public string move;
		public MoveLearnedDetails move_learned_details;
	}

	[System.Serializable]
	public class Stats
	{
		public int base_stat;
		public string stat;
	}

	[System.Serializable]
	public class Lander : IBaseModel
	{
		public int id;
		public string name;
		public string description;
		public List<Stats> stats;
		public int base_experience;
		public int base_height;
		public int base_weight;
		public List<string> types;
		public List<Moves> moves;
		public string model;
	}
}
