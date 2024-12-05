using System;
using System.Data.SqlTypes;

namespace LandersLegends.Extern.API
{
	[System.Serializable]
	public class MoveLearnedDetails
	{
		public int level_learned_at;
	}

	[System.Serializable]
	public class MoveAilement
	{
		public string ailement;
		public int chance;
	}

	[System.Serializable]
	public class Move : IBaseModel, ICloneable
	{
		public int id;
		public string name;
		public int accuracy;
		public int power;
		public int pp;
		public int priority;
		public string type;
		public MoveAilement move_ailement;

		public object Clone()
		{
			return new Move
			{
				id = this.id,
				name = this.name,
				accuracy = this.accuracy,
				power = this.power,
				pp = this.pp,
				priority = this.priority,
				type = this.type,
				move_ailement = this.move_ailement
			};
		}
	}
}
