using System.Linq;

namespace Lander.Module.API
{
	public class NatureRepository
	{
		private static Nature[] natureList;

		public static void Initialize()
		{
			natureList = DataFetcher<Nature>.FetchArrayData($"api/v1/nature");
		}

		public static Nature[] GetAll() => natureList;
		public static Nature GetByName(string name) => natureList.First(x => x.name == name);
		public static Nature GetById(int id) => natureList.First(x => x.id == id);
		public static int GetIdByName(string name) => natureList.First(x => x.name == name).id;
		public static string GetNameById(int id) => natureList.First(x => x.id == id).name;
		public static bool IsExist(string name) => natureList.Any(x => x.name.Equals(name));
	}
}
