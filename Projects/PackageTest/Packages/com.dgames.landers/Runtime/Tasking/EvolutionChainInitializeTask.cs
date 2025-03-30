using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class EvolutionChainInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            EvolutionChainRepository.Instance = new EvolutionChainRepository();
            bool result = await EvolutionChainRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
