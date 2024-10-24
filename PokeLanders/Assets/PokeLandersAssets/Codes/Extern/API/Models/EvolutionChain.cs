using System.Collections.Generic;

namespace Lander.Extern.API
{
	[System.Serializable]
	public class EvolutionDetails
	{
		public int? min_level;
	}

	[System.Serializable]
	public class Chain
	{
		public string species;
		public List<EvolutionDetails> evolution_details;
		public List<Chain> evolves_to;
	}

	[System.Serializable]
	public class EvolutionChain : IBaseModel
	{
		public int id;
		public Chain chain;
	}
}
