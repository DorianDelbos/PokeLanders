namespace Lander.API
{
	[System.Serializable]
	public class MoveLearnedDetails
	{
		public int LevelLearnedAt;
	}

	[System.Serializable]
	public class MoveAilement
	{
		public string Ailement;
		public int Chance;
	}

	[System.Serializable]
	public class Move : IBaseModel
	{
		public int ID;
		public string Name;
		public int Accuracy;
		public int Power;
		public int Pp;
		public int Priority;
		public string Type;
		public MoveAilement MoveAilement;
	}
}
