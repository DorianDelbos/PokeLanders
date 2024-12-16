using dgames.http;
using dgames.nfc;
using dgames.Utilities;
using Landers.API;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	public void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Y))
			return;

        ConsoleSystem.instance.AppendText($"Request web ...");
        AsyncOperationWeb<Lander> asyncOp = WebService.AsyncRequestJson<Lander>("http://localhost:5000/api/v1/lander/1");
		asyncOp.OnComplete += op =>
		{
			if (!op.IsError)
				ConsoleSystem.instance.AppendText($"Data received: {op.Result.name}");
			else
				ConsoleSystem.instance.AppendText($"Error occurred: {op.Exception.Message}", Color.red);
		};
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
