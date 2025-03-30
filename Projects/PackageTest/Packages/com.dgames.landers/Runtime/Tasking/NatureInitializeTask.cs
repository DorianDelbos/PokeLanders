using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class NatureInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            NatureRepository.Instance = new NatureRepository();
            bool result = await NatureRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
