using dgames.http;
using dgames.nfc;
using dgames.Utilities;
using Landers.API;
using System.IO;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	private WebService webService = new WebService();

	public void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Y))
			return;

		using (var webService = new WebService())
        {
            webService.AsyncRequestJson<Lander>(
                "https://localhost:5000/lander/1",
                (success, result, exception) =>
                {
                    if (success)
                    {
						ConsoleSystem.instance.AppendText($"Data received: {result}");
					}
                    else
					{
						ConsoleSystem.instance.AppendText($"Error occurred: {exception.Message}", Color.red);
                    }
                });
        }
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
