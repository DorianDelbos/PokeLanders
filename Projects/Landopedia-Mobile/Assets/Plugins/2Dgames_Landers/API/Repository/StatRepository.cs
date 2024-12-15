using dgames.http;
using NUnit.Framework.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Landers.API
{
	public static class StatRepository
	{
		private static Stat[] statList;

		public static void Initialize(Action<bool, Stat[], Exception> onCompleted)
		{
            try
            {
                WebService webService = new WebService();
                webService.AsyncRequestJson<Stat[]>(Path.Combine(ApiSettings.instance.ApiUrl, "api/v1/stat"), (isSucceed, stat, e) =>
                {
                    statList = stat;
                    onCompleted(isSucceed, stat, e);
                });
            }
            catch (Exception e)
            {
                onCompleted?.Invoke(false, null, e);
            }
        }

		public static Stat[] GetAll() => statList;
		public static Stat GetByName(string name) => statList.First(x => x.name == name);
		public static Stat GetById(int id) => statList.First(x => x.id == id);
		public static int GetIdByName(string name) => statList.First(x => x.name == name).id;
		public static string GetNameById(int id) => statList.First(x => x.id == id).name;
		public static bool IsExist(string name) => statList.Any(x => x.name.Equals(name));
	}
}
