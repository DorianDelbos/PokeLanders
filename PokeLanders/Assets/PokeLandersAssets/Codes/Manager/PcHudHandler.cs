using Lander.Gameplay.Type;
using Lander.Gameplay.Web;
using Lander.Maths;
using Lander.NFC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lander.Gameplay
{
	public class PcHudHandler : MonoBehaviour
	{
		[Header("Editing")]
		[SerializeField] private TMP_Text landopediaNumberMesh;
		[SerializeField] private TMP_Text speciesNameMesh;
		[SerializeField] private RectTransform typesTransform;
		[SerializeField] private TMP_Text descriptionMesh;
		[SerializeField] private TMP_Text heightWeightMesh;
		[SerializeField] private TMP_Text customNameMesh;
		[SerializeField] private Slider lifeBar;
		[SerializeField] private Slider xpBar;
		[SerializeField] private TMP_Text levelMesh;

		[Header("Panels")]
		[SerializeField] private GameObject dataPanel;
		[SerializeField] private GameObject bottomPanel;
		[SerializeField] private GameObject infoText;
		[SerializeField] private Button[] buttons;

		[Header("Other")]
		[SerializeField] private Sprite[] typesSprite;

		private void OnEnable()
		{
			NfcModule.onNewNfcDetect += UpdatePc;
			NfcModule.onNfcRemove += DisablePc;
		}

		private void OnDisable()
		{
			NfcModule.onNewNfcDetect -= UpdatePc;
			NfcModule.onNfcRemove -= DisablePc;
		}

		private void DisablePc(LanderDataNFC data) => ActivePc(false);

		private void ActivePc(bool active)
		{
			dataPanel.SetActive(active);
			bottomPanel.SetActive(active);
			infoText.SetActive(!active);

			foreach (var button in buttons)
			{
				button.interactable = active;
			}
		}

		private void UpdatePc(LanderDataNFC data)
		{
			ActivePc(true);
			LanderData landerData = new LanderData(data);

			WebRequests.instance.DoRequestAsync($"GetLandersById.php?ID={landerData.ID}", (fetch, e) =>
			{
				speciesNameMesh.text = fetch["Name"];
				descriptionMesh.text = fetch["Description"];

				foreach (Transform child in typesTransform)
				{
					Destroy(child.gameObject);
				}

				string[] rows = fetch["Types"].Split(",");
				foreach (string row in rows)
				{
					ElementaryType type = (ElementaryType)System.Enum.Parse(typeof(ElementaryType), row);
					GameObject go = new GameObject("type");
					go.transform.parent = typesTransform;
					go.transform.localScale = Vector3.one;
					Image image = go.AddComponent<Image>();
					image.rectTransform.sizeDelta = new Vector2(84, 20);
					image.sprite = typesSprite[(int)type];
				}
			});

			landopediaNumberMesh.text = $"Landopedia No. {landerData.ID.ToString("D3")}";
			heightWeightMesh.text = $"Height \t {GetHeightDisplay(landerData.Height)}\nWeight \t {GetWeightDisplay(landerData.Weight)} lbs";
			customNameMesh.text = landerData.CustomName;
			lifeBar.maxValue = landerData.MaxHp;
			lifeBar.value = landerData.Hp;
			xpBar.maxValue = StatsCurves.GetXpByLevel(landerData.Level + 1);
			xpBar.value = landerData.Xp;
			levelMesh.text = $"Lvl.{landerData.Level}";
		}

		private string GetHeightDisplay(ushort height)
		{
			int kilograms = height / 100;
			int grams = height % 100;

			return $"{kilograms}'{grams:D2}''";
		}

		private string GetWeightDisplay(ushort weight)
		{
			return (weight / 100m).ToString("F2");
		}
	}
}
