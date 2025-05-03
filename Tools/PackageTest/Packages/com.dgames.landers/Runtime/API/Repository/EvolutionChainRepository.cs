using dgames.http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landers.API
{
    public class EvolutionChainRepository : BaseRepository<EvolutionChain>
    {
        public static EvolutionChainRepository Instance { get; set; }

        public override async Task<bool> Initialize()
        {
            string request = Path.Combine(LanderConst.Url, "api/v1/evolutionChain");
            AsyncOperationWeb<EvolutionChain[]> asyncOp = WebService.AsyncRequestJson<EvolutionChain[]>(request);

            await asyncOp.AwaitCompletion();

            if (!asyncOp.IsError)
            {
                modelList = asyncOp.Result;
                return true;
            }

            return false;
        }

        #region Internal methods

        public EvolutionChain GetById(int id) => modelList.First(x => x.id == id);

        #endregion
    }
}
