using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Lander.Module.Utilities
{
    public static class WebSpriteUtilities
    {
        public static async Task<Sprite> LoadSpriteFromUrlAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                var asyncOp = request.SendWebRequest();

                while (!asyncOp.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load image from URL: {request.error}");
                    return null;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = TextureToSprite(texture);
                return sprite;
            }
        }

        public static Sprite LoadSpriteFromUrl(string url) => Task.Run(async () => await LoadSpriteFromUrlAsync(url)).Result;

        private static Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
