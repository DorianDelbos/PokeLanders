using System.Collections.Generic;

namespace Lander.API
{
	[System.Serializable]
	public class Moves
	{
		public string move;
		public MoveLearnedDetails moveLearnedDetails;
	}

	[System.Serializable]
	public class Stats
	{
		public int baseStat;
		public string stat;
	}

	[System.Serializable]
	public class Lander : IBaseModel
	{
		public int id;
		public string name;
		public string description;
		public List<Stats> stats;
		public int baseExperience;
		public int baseHeight;
		public int baseWeight;
		public List<string> types;
		public List<Moves> moves;
		public string model;
	}
}
