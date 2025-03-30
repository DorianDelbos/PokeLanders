using dgames.nfc;
using dgames.Tasks;
using dgames.Utils;
using Landers;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private TaskManager taskManager;

    private void Awake()
    {
        taskManager = new TaskManager();
    }

    public async void InitializeLander()
    {
        ConsoleSystem.instance.AppendText($"Request web ...");

        bool taskWaiting = taskManager.GetTaskStackCount() > 0;

        if (!taskWaiting)
        {
            taskManager.EnqueueTask(new AilmentInitializeTask());
            taskManager.EnqueueTask(new EvolutionChainInitializeTask());
            taskManager.EnqueueTask(new LanderInitializeTask());
            taskManager.EnqueueTask(new MoveInitializeTask());
            taskManager.EnqueueTask(new NatureInitializeTask());
            taskManager.EnqueueTask(new StatInitializeTask());
            taskManager.EnqueueTask(new TypeInitializeTask());
        }

        await taskManager.StartProcessingAsync();

        taskWaiting = taskManager.GetTaskStackCount() > 0;
        if (taskWaiting)
            ConsoleSystem.instance.AppendText($"Requests failed", Color.red);
        else
            ConsoleSystem.instance.AppendText($"Requests succeed", Color.green);
    }

    public void NFCTagTest()
    {
        ConsoleSystem.instance.AppendText($"Wait for NFC ...");
        AsyncOperationNfc operation = NFCSystem.ReadTag(5000);
        operation.OnComplete += op =>
        {
            if (!op.IsError)
                ConsoleSystem.instance.AppendText($"Tag result : {op.Result.ToHex()}");
            else
                ConsoleSystem.instance.AppendText($"Error : {op.Exception.Message}", Color.red);
        };
    }

    public void NFCBlockTest()
    {
        ConsoleSystem.instance.AppendText($"Wait for NFC ...");
        AsyncOperationNfc operation = NFCSystem.ReadBlock(0, 0, 5000);
        operation.OnComplete += op =>
        {
            if (!op.IsError)
                ConsoleSystem.instance.AppendText($"Tag result : {op.Result.ToHex()}");
            else
                ConsoleSystem.instance.AppendText($"Error : {op.Exception.Message}", Color.red);
        };
    }
}
