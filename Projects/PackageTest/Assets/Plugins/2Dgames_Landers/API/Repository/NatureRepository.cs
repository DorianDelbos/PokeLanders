using dgames.http;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public class NatureRepository
	{
		private static Nature[] natureList;

		public static void Initialize()
		{
			WebService webService = new WebService();
			webService.AsyncRequestJson<Nature[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/nature"), (isSucceed, nature) =>
			{
				natureList = nature;
			});
		}

		public static Nature[] GetAll() => natureList;
		public static Nature GetByName(string name) => natureList.First(x => x.name == name);
		public static Nature GetById(int id) => natureList.First(x => x.id == id);
		public static int GetIdByName(string name) => natureList.First(x => x.name == name).id;
		public static string GetNameById(int id) => natureList.First(x => x.id == id).name;
		public static bool IsExist(string name) => natureList.Any(x => x.name.Equals(name));
	}
}
