using dgames.http;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
    public static class AilmentRepository
    {
		private static Ailment[] ailmentList;

		public static void Initialize(Action<bool, Ailment[], Exception> onCompleted)
		{
            try
            {
                WebService webService = new WebService();
                webService.AsyncRequestJson<Ailment[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/ailment"), (isSucceed, ailment, e) =>
                {
                    ailmentList = ailment;
                    onCompleted?.Invoke(isSucceed, ailment, e);
                });
            }
            catch (Exception e)
            {
                onCompleted?.Invoke(false, null, e);
            }
        }

		public static Ailment[] GetAll() => ailmentList;
		public static Ailment GetByName(string name) => ailmentList.First(x => x.name == name);
		public static Ailment GetById(int id) => ailmentList.First(x => x.id == id);
		public static int GetIdByName(string name) => ailmentList.First(x => x.name == name).id;
		public static string GetNameById(int id) => ailmentList.First(x => x.id == id).name;
		public static bool IsExist(string name) => ailmentList.Any(x => x.name.Equals(name));
	}
}
