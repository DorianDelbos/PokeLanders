using System.Linq;

namespace LandersLegends.Extern.API
{
	public static class LanderRepository
	{
		private static Lander[] landerList;

		public static void Initialize()
		{
			landerList = DataFetcher<Lander>.FetchArrayData($"api/v1/lander");
		}

		public static Lander[] GetAll() => landerList;
		public static Lander GetByName(string name) => landerList.First(x => x.name == name);
		public static Lander GetById(int id) => landerList.First(x => x.id == id);
		public static int GetIdByName(string name) => landerList.First(x => x.name == name).id;
		public static string GetNameById(int id) => landerList.First(x => x.id == id).name;
		public static bool IsExist(string name) => landerList.Any(x => x.name.Equals(name));
	}
}
