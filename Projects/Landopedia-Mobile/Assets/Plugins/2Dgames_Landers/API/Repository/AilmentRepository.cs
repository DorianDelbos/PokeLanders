using dgames.http;
using System;
using System.IO;
using System.Linq;
using Unity.Collections;

namespace Landers.API
{
    public static class AilmentRepository
    {
		private static Ailment[] ailmentList;

		public static AsyncOperationWeb<Ailment[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/ailement");
			AsyncOperationWeb<Ailment[]> asyncOp = WebService.AsyncRequestJson<Ailment[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<Ailment[]> operation)
		{
			if (operation.Exception != null)
				return;

			ailmentList = operation.Result;
		}

		public static Ailment[] GetAll() => ailmentList;
		public static Ailment GetByName(string name) => ailmentList.First(x => x.name == name);
		public static Ailment GetById(int id) => ailmentList.First(x => x.id == id);
		public static int GetIdByName(string name) => ailmentList.First(x => x.name == name).id;
		public static string GetNameById(int id) => ailmentList.First(x => x.id == id).name;
		public static bool IsExist(string name) => ailmentList.Any(x => x.name.Equals(name));
	}
}
