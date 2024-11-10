using System.Linq;

namespace Dgames.Extern.API
{
	public static class AilmentRepository
	{
		private static Ailment[] ailmentList;

		public static void Initialize()
		{
			ailmentList = DataFetcher<Ailment>.FetchArrayData($"api/v1/ailment");
		}

		public static Ailment[] GetAll() => ailmentList;
		public static Ailment GetByName(string name) => ailmentList.First(x => x.name == name);
		public static Ailment GetById(int id) => ailmentList.First(x => x.id == id);
		public static int GetIdByName(string name) => ailmentList.First(x => x.name == name).id;
		public static string GetNameById(int id) => ailmentList.First(x => x.id == id).name;
		public static bool IsExist(string name) => ailmentList.Any(x => x.name.Equals(name));
	}
}
