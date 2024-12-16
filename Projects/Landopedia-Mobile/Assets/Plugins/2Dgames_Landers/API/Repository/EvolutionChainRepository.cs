using dgames.http;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class EvolutionChainRepository
	{
		private static EvolutionChain[] evolutionChainList;

		public static AsyncOperationWeb<EvolutionChain[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/evolutionChain");
			AsyncOperationWeb<EvolutionChain[]> asyncOp = WebService.AsyncRequestJson<EvolutionChain[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<EvolutionChain[]> operation)
		{
			if (operation.Exception != null)
				throw operation.Exception;

			evolutionChainList = operation.Result;
		}

		public static EvolutionChain[] GetAll() => evolutionChainList;
		public static EvolutionChain GetById(int id) => evolutionChainList.First(x => x.id == id);
	}
}
