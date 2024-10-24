using Lander.Gameplay.Type;
using Lander.Maths;
using Lander.Extern;
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

		public void UpdatePc(LanderData data, bool active)
		{
			ActivePc(active);
			if (!active) return;

			landopediaNumberMesh.text = $"Landopedia No. {data.ID.ToString("D3")}";
			speciesNameMesh.text = data.Species;
			descriptionMesh.text = data.Description;
			heightWeightMesh.text = $"Height \t {GetHeightDisplay(data.Height)}\nWeight \t {GetWeightDisplay(data.Weight)} lbs";
			customNameMesh.text = data.Name;
			lifeBar.maxValue = data.MaxHp;
			lifeBar.value = data.Hp;
			xpBar.maxValue = StatsCurves.GetXpByLevel((byte)(data.Level + 1), data.BaseXp);
			xpBar.value = data.Xp;
			levelMesh.text = $"Lvl.{data.Level}";

			foreach (Transform child in typesTransform)
			{
				Destroy(child.gameObject);
			}

			foreach (ElementaryType type in data.Types)
			{
				GameObject go = new GameObject("type");
				go.transform.parent = typesTransform;
				go.transform.localScale = Vector3.one;
				Image image = go.AddComponent<Image>();
				image.rectTransform.sizeDelta = new Vector2(84, 20);
				image.sprite = typesSprite[(int)type];
			}
		}

		private string GetHeightDisplay(int height)
		{
			int kilograms = height / 100;
			int grams = height % 100;

			return $"{kilograms}'{grams:D2}''";
		}

		private string GetWeightDisplay(int weight)
		{
			return (weight / 100m).ToString("F2");
		}
	}
}
