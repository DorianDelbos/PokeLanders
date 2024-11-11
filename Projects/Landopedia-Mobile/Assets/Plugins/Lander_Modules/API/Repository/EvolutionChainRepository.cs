using System.Linq;

namespace Lander.Module.API
{
	public static class EvolutionChainRepository
	{
		private static EvolutionChain[] evolutionChainList;

		public static void Initialize()
		{
			evolutionChainList = DataFetcher<EvolutionChain>.FetchArrayData($"api/v1/evolutionChain");
		}

		public static EvolutionChain[] GetAll() => evolutionChainList;
		public static EvolutionChain GetById(int id) => evolutionChainList.First(x => x.id == id);
	}
}
