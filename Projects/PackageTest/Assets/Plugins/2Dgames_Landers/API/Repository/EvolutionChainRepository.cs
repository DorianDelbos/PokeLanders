using dgames.http;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class EvolutionChainRepository
	{
		private static EvolutionChain[] evolutionChainList;

		public static void Initialize()
		{
			WebService webService = new WebService();
			webService.AsyncRequestJson<EvolutionChain[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/evolutionChain"), (isSucceed, evolutionChain) =>
			{
				evolutionChainList = evolutionChain;
			});
		}

		public static EvolutionChain[] GetAll() => evolutionChainList;
		public static EvolutionChain GetById(int id) => evolutionChainList.First(x => x.id == id);
	}
}
