using LandersLegends.Extern.API;
using LandersLegends.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LandersLegends.Battle
{
	public class AttackUIHandler : MonoBehaviour
	{
		[SerializeField] private Image colorImage;
		[SerializeField] private RawImage image;
        [SerializeField] private TMP_Text nameTextMesh;
		[SerializeField] private TMP_Text ppTextMesh;

		private Move move;
		private Type type;

		private BattleStateMachine battlesStateMachine => BattleManager.instance.StateMachine;

		public void InitializeUI(Move move)
		{
			this.move = move;
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
            ppTextMesh.text = $"{move.pp} / {MoveRepository.GetById(move.id).pp}";
        }

		private void Use()
		{
			if (move.pp <= 0)
				return;

			move.pp--;
			battlesStateMachine.CurrentLander.lastMoveProccess = (ushort)move.id;
			battlesStateMachine.ProcessNextState();
		}
	}
}
