using dgames.http;
using dgames.nfc;
using dgames.Utilities;
using Landers.API;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	private WebService webService = new WebService();

	public void RequestTest()
	{
		ConsoleSystem.instance.AppendText($"Request web launch ...");
		string request = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/api/v1/lander/1";
		webService.AsyncRequestJson<Lander>(request, (isSucceed, lander) =>
		{
			if (isSucceed)
			{
				ConsoleSystem.instance.AppendText($"Lander {lander.name} was correctly load !");
			}
			else
			{
				ConsoleSystem.instance.AppendText("An error was appear when you tried to load a lander !", Color.red);
			}
		});
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
