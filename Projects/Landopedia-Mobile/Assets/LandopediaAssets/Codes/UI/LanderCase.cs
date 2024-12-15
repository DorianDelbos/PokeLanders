using Landers.API;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Landopedia
{
	public class LanderCase : MonoBehaviour
    {
        [SerializeField] private TMP_Text landerIDTextMesh;
        [SerializeField] private RawImage landerImage;
        private Lander lander;
        private bool hasLander = false;

        public bool HasLander
        {
            get => hasLander;
            set
            {
                hasLander = value;
                landerIDTextMesh.gameObject.SetActive(!hasLander);
                landerImage.gameObject.SetActive(hasLander);
            }
        }

        public void SetLander(Lander lander)
        {
            this.lander = lander;
            landerIDTextMesh.text = lander.id.ToString("D3");
        }

        public void SetTexture(Texture2D texture)
        {
            landerImage.texture = texture;
        }

        public void OpenLander()
        {
            if (!hasLander)
                return;

            LanderMenuHandler.lander = lander;
            SceneManager.LoadScene("Lander");
        }
    }
}
