using dgames.http;
using Landers.API;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	private WebService webService = new WebService();

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			string request = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/api/v1/lander/1";
			webService.AsyncRequestJson<Lander>(request, (isSucceed, lander) =>
			{
				if (isSucceed)
				{
					Debug.Log($"Lander {lander.name} was correctly load !");
				}
				else
				{
					Debug.Log("An error was appear when you tried to load a lander !");
				}
			});
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			string request = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/api/v1/lander";
			webService.AsyncRequestJson<Lander[]>(request, (isSucceed, lander) =>
			{
				if (isSucceed)
				{
					Debug.Log($"{lander.Length} landers was correctly load !");
				}
				else
				{
					Debug.Log("An error was appear when you tried to load a lander array !");
				}
			});
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            string request = "https://landopedia-gwhtbqbqdhd4d5hw.westeurope-01.azurewebsites.net/api/v1/sprite/aquapix.png";
            webService.AsyncRequestImage(request, (isSucceed, texture) =>
            {
                if (isSucceed)
                {
                    Debug.Log($"The texture {texture.name} was correctly load !");
                }
                else
                {
                    Debug.Log("An error was appear when you tried to load a texture !");
                }
            });
        }
    }
}
