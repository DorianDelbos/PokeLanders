using TMPro;
using UnityEngine;

namespace LandersLegends.Gameplay
{
	public class NfcErrorHudManager : MonoBehaviour
	{
		public static NfcErrorHudManager current;

		[SerializeField] private GameObject canvas;
		[SerializeField] private TMP_Text textMesh;
		private bool isActive = false;

		public bool IsActive => isActive;

		private void Awake()
		{
			if (current != null)
			{
				Debug.LogWarning($"Anoter instance of {name} already exist !");
				return;
			}
			current = this;
		}

		public void SetActive(bool active)
		{
			canvas.SetActive(active);
			isActive = active;
		}

		public void SetErrorText(string text)
		{
			textMesh.text = text;
		}
	}
}
