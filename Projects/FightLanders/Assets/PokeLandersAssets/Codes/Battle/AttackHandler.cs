using LandersLegends.Extern.API;
using LandersLegends.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LandersLegends.Battle
{
	public class AttackHandler : MonoBehaviour
	{
		[SerializeField] private Image colorImage;
		[SerializeField] private RawImage image;
        [SerializeField] private TMP_Text nameTextMesh;
		[SerializeField] private TMP_Text ppTextMesh;

		private Move move;
		private Type type;
		private int currentPP;

        public void InitializeByID(ushort id)
		{
			move = MoveRepository.GetById(id);
			type = TypeRepository.GetByName(move.type);

			UpdateUI();

            Button buttonAttack = GetComponent<Button>();
            buttonAttack.onClick.AddListener(Use);
        }

		private void UpdateUI()
        {
            colorImage.color = type.color.ToColor();
            image.texture = Resources.Load<Texture>($"ElementaryIcons/{move.type}");
            nameTextMesh.text = move.name;
            ppTextMesh.text = $"{move.pp} / {move.pp}";
        }

		// TODO : this is delete between all states
		// Change logic
		private void Use()
		{
			if (currentPP <= 0)
				return;

			currentPP--;
			UpdateUI();
            BattleStateMachine.Instance.ProcessNextState();
        }
	}
}
