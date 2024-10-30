using LandersLegends.Extern.API;
using LandersLegends.Maths;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LandersLegends.Gameplay
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

		[Header("Other")]
		[SerializeField] private Sprite[] typesSprite;

		private void ActivePc(bool active)
		{
            NfcErrorHudManager.current.SetActive(!active);

			if (!active)
				NfcErrorHudManager.current.SetErrorText("Place a Lander in the field !");
		}

		public void UpdatePc(Lander data, bool active)
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
			xpBar.value = data.Xp - StatsCurves.GetXpByLevel(data.Level, data.BaseXp);
			levelMesh.text = $"Lvl.{data.Level}";

			foreach (Transform child in typesTransform)
			{
				Destroy(child.gameObject);
			}

			foreach (string type in data.Types)
			{
				GameObject go = new GameObject("type");
				go.transform.parent = typesTransform;
				go.transform.localScale = Vector3.one;
				Image image = go.AddComponent<Image>();
				image.rectTransform.sizeDelta = new Vector2(84, 20);
				image.sprite = typesSprite[TypeRepository.GetIdByName(type) - 1];
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
