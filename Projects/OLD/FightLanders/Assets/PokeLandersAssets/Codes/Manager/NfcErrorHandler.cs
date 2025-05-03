using TMPro;
using UnityEngine;

namespace LandersLegends.Gameplay
{
	public class NfcErrorHandler : MonoBehaviour
	{
		public static NfcErrorHandler current;

		[SerializeField] private GameObject canvas;
		[SerializeField] private TMP_Text textMesh;

		public bool IsActive => canvas.activeSelf;

		private void Awake()
		{
			if (current != null)
			{
				Debug.LogWarning($"Anoter instance of {name} already exist !");
				return;
			}
			current = this;
		}

		public void CallError(string text)
		{
			canvas.SetActive(true);
			textMesh.text = text;
		}

		public void Close()
		{
			canvas.SetActive(false);
		}
	}
}
