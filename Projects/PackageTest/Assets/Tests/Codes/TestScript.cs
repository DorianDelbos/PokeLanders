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

		AsyncOperationWeb<Lander> asyncOp = WebService.AsyncRequestJson<Lander>("http://localhost:5000/api/v1/lander/1");
		asyncOp.OnComplete += op =>
		{
			if (op.Exception == null)
				ConsoleSystem.instance.AppendText($"Data received: {op.Result.name}");
			else
				ConsoleSystem.instance.AppendText($"Error occurred: {op.Exception.Message}", Color.red);
		};
	}

	public void NFCTagTest()
	{
		ConsoleSystem.instance.AppendText($"Wait for NFC ...");
		NFCSystem NFCSystem = new NFCSystem() { timeout = 5000 };
		NFCSystem.ReadTagAsync((isSucceed, result, e) =>
		{
			if (isSucceed)
			{
				ConsoleSystem.instance.AppendText($"Tag result : {result.ToHex()}");
			}
			else
			{
				ConsoleSystem.instance.AppendText($"Error : {e.Message}", Color.red);
			}
		});
	}

	public void NFCBlockTest()
	{
		ConsoleSystem.instance.AppendText($"Wait for NFC ...");
		NFCSystem NFCSystem = new NFCSystem() { timeout = 5000 };
		NFCSystem.ReadBlockAsync(0, 0, (isSucceed, result, e) =>
		{
			if (isSucceed)
			{
				ConsoleSystem.instance.AppendText($"Tag result : {result.ToHex()}");
			}
			else
			{
				ConsoleSystem.instance.AppendText($"Error : {e.Message}", Color.red);
			}
		});
	}
}
