using dgames.http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landers.API
{
    public class MoveRepository : BaseRepository<Move>
    {
        public static MoveRepository Instance { get; set; }

        public override async Task<bool> Initialize()
        {
            string request = Path.Combine(LanderConst.Url, "api/v1/move");
            AsyncOperationWeb<Move[]> asyncOp = WebService.AsyncRequestJson<Move[]>(request);

            await asyncOp.AwaitCompletion();

            if (!asyncOp.IsError)
            {
                modelList = asyncOp.Result;
                return true;
            }

            return false;
        }

        #region Internal models

        public Move GetByName(string name) => modelList.First(x => x.name == name);
        public Move GetById(int id) => modelList.First(x => x.id == id);
        public int GetIdByName(string name) => modelList.First(x => x.name == name).id;
        public string GetNameById(int id) => modelList.First(x => x.id == id).name;
        public bool IsExist(string name) => modelList.Any(x => x.name.Equals(name));

        #endregion
    }
}
