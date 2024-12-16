using dgames.http;
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

namespace Landers.API
{
	public static class MoveRepository
	{
		private static Move[] moveList;

		public static AsyncOperationWeb<Move[]> Initialize()
		{
			string request = Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/move");
			AsyncOperationWeb<Move[]> asyncOp = WebService.AsyncRequestJson<Move[]>(request);
			asyncOp.OnComplete += OnInitialize;
			return asyncOp;
		}

		public static void OnInitialize(AsyncOperationWeb<Move[]> operation)
		{
			if (operation.Exception != null)
				throw operation.Exception;

			moveList = operation.Result;
		}

		public static Move[] GetAll() => moveList;
		public static Move GetByName(string name) => moveList.First(x => x.name == name);
		public static Move GetById(int id) => moveList.First(x => x.id == id);
		public static int GetIdByName(string name) => moveList.First(x => x.name == name).id;
		public static string GetNameById(int id) => moveList.First(x => x.id == id).name;
		public static bool IsExist(string name) => moveList.Any(x => x.name.Equals(name));
	}
}
