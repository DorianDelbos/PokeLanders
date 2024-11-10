namespace Dgames.Extern.API
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
	public class Move : IBaseModel
	{
		public int id;
		public string name;
		public int accuracy;
		public int power;
		public int pp;
		public int priority;
		public string type;
		public MoveAilement move_ailement;
	}
}
