using dgames.http;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class StatRepository
	{
		private static Stat[] statList;

		public static AsyncOperationWeb<Stat[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/stat");
			AsyncOperationWeb<Stat[]> asyncOp = WebService.AsyncRequestJson<Stat[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<Stat[]> operation)
		{
			if (operation.Exception != null)
				throw operation.Exception;

			statList = operation.Result;
		}

		public static Stat[] GetAll() => statList;
		public static Stat GetByName(string name) => statList.First(x => x.name == name);
		public static Stat GetById(int id) => statList.First(x => x.id == id);
		public static int GetIdByName(string name) => statList.First(x => x.name == name).id;
		public static string GetNameById(int id) => statList.First(x => x.id == id).name;
		public static bool IsExist(string name) => statList.Any(x => x.name.Equals(name));
	}
}
