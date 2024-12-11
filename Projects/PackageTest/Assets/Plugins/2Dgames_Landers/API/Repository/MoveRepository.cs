using dgames.http;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class MoveRepository
	{
		private static Move[] moveList;

		public static void Initialize()
		{
			WebService webService = new WebService();
			webService.AsyncRequestJson<Move[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/move"), (isSucceed, move) =>
			{
				moveList = move;
			});
		}

		public static Move[] GetAll() => moveList;
		public static Move GetByName(string name) => moveList.First(x => x.name == name);
		public static Move GetById(int id) => moveList.First(x => x.id == id);
		public static int GetIdByName(string name) => moveList.First(x => x.name == name).id;
		public static string GetNameById(int id) => moveList.First(x => x.id == id).name;
		public static bool IsExist(string name) => moveList.Any(x => x.name.Equals(name));
	}
}
