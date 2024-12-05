using System.Linq;

namespace LandersLegends.Extern.API
{
	public static class MoveRepository
	{
		private static Move[] moveList;

		public static void Initialize()
		{
			moveList = DataFetcher<Move>.FetchArrayData($"api/v1/move");
		}

		public static Move[] GetAll() => moveList.Select(x => x.Clone() as Move).ToArray();
		public static Move GetByName(string name) => moveList.FirstOrDefault(x => x.name == name)?.Clone() as Move;
		public static Move GetById(int id) => moveList.FirstOrDefault(x => x.id == id)?.Clone() as Move;
		public static int GetIdByName(string name) => moveList.FirstOrDefault(x => x.name == name).id;
		public static string GetNameById(int id) => moveList.FirstOrDefault(x => x.id == id).name;
		public static bool IsExist(string name) => moveList.Any(x => x.name.Equals(name));
	}
}
