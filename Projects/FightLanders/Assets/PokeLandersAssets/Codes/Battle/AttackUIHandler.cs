using LandersLegends.Extern.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LandersLegends.Battle
{
	public class AttackUIHandler : MonoBehaviour
	{
		[SerializeField] private RawImage image;
		[SerializeField] private TMP_Text nameTextMesh;
		[SerializeField] private TMP_Text ppTextMesh;

		public void UpdateByID(ushort id)
		{
			Move move = MoveRepository.GetById(id);
			image.texture = Resources.Load<Texture>($"ElementaryIcon/{move.type}");
			nameTextMesh.text = move.name;
			ppTextMesh.text = $"{move.pp} / {move.pp}";
		}
	}
}
