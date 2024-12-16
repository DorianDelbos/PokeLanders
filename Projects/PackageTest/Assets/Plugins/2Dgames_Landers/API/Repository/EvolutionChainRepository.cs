using dgames.http;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class EvolutionChainRepository
	{
		private static EvolutionChain[] evolutionChainList;

		public static void Initialize(Action<bool, EvolutionChain[], Exception> onCompleted)
		{
            try
            {
                WebService webService = new WebService();
                webService.AsyncRequestJson<EvolutionChain[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/evolutionChain"), (isSucceed, evolutionChain, e) =>
                {
                    evolutionChainList = evolutionChain;
                    onCompleted?.Invoke(isSucceed, evolutionChain, e);

                });
            }
            catch (Exception e)
            {
                onCompleted?.Invoke(false, null, e);
            }
        }

		public static EvolutionChain[] GetAll() => evolutionChainList;
		public static EvolutionChain GetById(int id) => evolutionChainList.First(x => x.id == id);
	}
}
