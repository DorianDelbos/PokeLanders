using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class MoveInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            MoveRepository.Instance = new MoveRepository();
            bool result = await MoveRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
