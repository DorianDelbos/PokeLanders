using dgames.http;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class LanderRepository
	{
		private static Lander[] landerList;

		public static AsyncOperationWeb<Lander[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/lander");
			AsyncOperationWeb<Lander[]> asyncOp = WebService.AsyncRequestJson<Lander[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<Lander[]> operation)
		{
			if (operation.Exception != null)
				throw operation.Exception;

			landerList = operation.Result;
		}

		public static Lander[] GetAll() => landerList;
		public static Lander GetByName(string name) => landerList.First(x => x.name == name);
		public static Lander GetById(int id) => landerList.First(x => x.id == id);
		public static int GetIdByName(string name) => landerList.First(x => x.name == name).id;
		public static string GetNameById(int id) => landerList.First(x => x.id == id).name;
		public static bool IsExist(string name) => landerList.Any(x => x.name.Equals(name));
	}
}
