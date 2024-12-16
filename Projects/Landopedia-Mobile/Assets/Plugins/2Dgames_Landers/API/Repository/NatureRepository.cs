using dgames.http;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public class NatureRepository
	{
		private static Nature[] natureList;

		public static AsyncOperationWeb<Nature[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/nature");
			AsyncOperationWeb<Nature[]> asyncOp = WebService.AsyncRequestJson<Nature[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<Nature[]> operation)
		{
			if (operation.Exception != null)
				throw operation.Exception;

			natureList = operation.Result;
		}

		public static Nature[] GetAll() => natureList;
		public static Nature GetByName(string name) => natureList.First(x => x.name == name);
		public static Nature GetById(int id) => natureList.First(x => x.id == id);
		public static int GetIdByName(string name) => natureList.First(x => x.name == name).id;
		public static string GetNameById(int id) => natureList.First(x => x.id == id).name;
		public static bool IsExist(string name) => natureList.Any(x => x.name.Equals(name));
	}
}
