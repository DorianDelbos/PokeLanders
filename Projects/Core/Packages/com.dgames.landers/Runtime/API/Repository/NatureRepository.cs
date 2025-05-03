using dgames.http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landers.API
{
    public class NatureRepository : BaseRepository<Nature>
    {
        public static NatureRepository Instance { get; set; }

        public override async Task<bool> Initialize()
        {
            string request = Path.Combine(LanderConst.Url, "api/v1/nature");
            AsyncOperationWeb<Nature[]> asyncOp = WebService.AsyncRequestJson<Nature[]>(request);

            await asyncOp.AwaitCompletion();

            if (!asyncOp.IsError)
            {
                modelList = asyncOp.Result;
                return true;
            }

            return false;
        }

        #region Internal methods

        public Nature GetByName(string name) => modelList.First(x => x.name == name);
        public Nature GetById(int id) => modelList.First(x => x.id == id);
        public int GetIdByName(string name) => modelList.First(x => x.name == name).id;
        public string GetNameById(int id) => modelList.First(x => x.id == id).name;
        public bool IsExist(string name) => modelList.Any(x => x.name.Equals(name));

        #endregion
    }
}
