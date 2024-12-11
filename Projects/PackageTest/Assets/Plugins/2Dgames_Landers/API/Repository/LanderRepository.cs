using dgames.http;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class LanderRepository
	{
		private static Lander[] landerList;

		public static void Initialize()
		{
			WebService webService = new WebService();
			webService.AsyncRequestJson<Lander[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/lander"), (isSucceed, lander) =>
			{
				landerList = lander;
			});
		}

		public static Lander[] GetAll() => landerList;
		public static Lander GetByName(string name) => landerList.First(x => x.name == name);
		public static Lander GetById(int id) => landerList.First(x => x.id == id);
		public static int GetIdByName(string name) => landerList.First(x => x.name == name).id;
		public static string GetNameById(int id) => landerList.First(x => x.id == id).name;
		public static bool IsExist(string name) => landerList.Any(x => x.name.Equals(name));
	}
}
