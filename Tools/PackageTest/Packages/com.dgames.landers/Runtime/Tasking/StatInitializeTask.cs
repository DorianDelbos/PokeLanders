using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class StatInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            StatRepository.Instance = new StatRepository();
            bool result = await StatRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
