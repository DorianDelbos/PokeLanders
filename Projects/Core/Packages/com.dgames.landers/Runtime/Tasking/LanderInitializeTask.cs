using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class LanderInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            LanderRepository.Instance = new LanderRepository();
            bool result = await LanderRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
