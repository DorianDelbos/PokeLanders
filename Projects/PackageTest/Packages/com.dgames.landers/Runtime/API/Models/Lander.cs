using System.Collections.Generic;

namespace Landers.API
{
	[System.Serializable]
	public class Moves
	{
		public string move;
		public MoveLearnedDetails move_learned_details;
	}

	[System.Serializable]
	public class BaseStats
	{
		public byte base_stat;
		public string stat;
	}

	[System.Serializable]
	public class Lander : IBaseModel
	{
		public ushort id;
		public string name;
		public string description;
		public List<BaseStats> stats;
		public ushort base_experience;
		public ushort base_height;
		public ushort base_weight;
		public List<string> types;
		public List<Moves> moves;
		public string model;
		public string sprite;
    }
}
