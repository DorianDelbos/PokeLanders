using dgames.http;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class MoveRepository
	{
		private static Move[] moveList;

		public static void Initialize(Action<bool, Move[], Exception> onCompleted)
		{
            try
            {
                WebService webService = new WebService();
                webService.AsyncRequestJson<Move[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/move"), (isSucceed, move, e) =>
                {
                    moveList = move;
                    onCompleted?.Invoke(isSucceed, move, e);
                });
            }
            catch (Exception e)
            {
                onCompleted?.Invoke(false, null, e);
            }
        }

		public static Move[] GetAll() => moveList;
		public static Move GetByName(string name) => moveList.First(x => x.name == name);
		public static Move GetById(int id) => moveList.First(x => x.id == id);
		public static int GetIdByName(string name) => moveList.First(x => x.name == name).id;
		public static string GetNameById(int id) => moveList.First(x => x.id == id).name;
		public static bool IsExist(string name) => moveList.Any(x => x.name.Equals(name));
	}
}
