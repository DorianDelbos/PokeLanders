using System.Collections.Generic;

namespace Lander.API
{
	[System.Serializable]
	public class EvolutionDetails
	{
		public int? minLevel;
	}

	[System.Serializable]
	public class Chain
	{
		public string species;
		public List<EvolutionDetails> evolutionDetails;
		public List<Chain> evolvesTo;
	}

	[System.Serializable]
	public class EvolutionChain : IBaseModel
	{
		public int id;
		public Chain chain;
	}
}
