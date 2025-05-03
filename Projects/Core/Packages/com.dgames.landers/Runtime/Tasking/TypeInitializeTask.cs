using dgames.Tasks;
using Landers.API;
using System.Threading.Tasks;

namespace Landers
{
    public class TypeInitializeTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            TypeRepository.Instance = new TypeRepository();
            bool result = await TypeRepository.Instance.Initialize();
            Result = result ? TaskResult.Success : TaskResult.WaitResponse;
        }
    }
}
