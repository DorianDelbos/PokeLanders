using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class AilmentInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            AilmentRepository.Instance = new AilmentRepository();
            bool result = await AilmentRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
