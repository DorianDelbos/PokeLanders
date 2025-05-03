using LandersLegends.Extern.API;
using LandersLegends.Gameplay;
using TMPro;
using UnityEngine;

namespace LandersLegends.Battle
{
	public class BattleHUDHandler : MonoBehaviour
    {
        [Header("Prefabs")]
		[SerializeField] private GameObject attackUIPrefab;

		[Header("UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private GameObject attackPanel;
        [SerializeField] private BattleLanderHudHandler[] hudHandler = new BattleLanderHudHandler[2];
        [SerializeField] private TMP_Dialogue dialogueMesh;

        private Gameplay.Lander[] landers => GameManager.instance.Landers;

        public void UpdateLandersHUD()
        {
            for (int i = 0; i < landers.Length; i++)
            {
                hudHandler[i].UpdateData(landers[i]);
            }
		}

        public bool UpdateDialogue()
		{
			bool isReading = dialogueMesh.IsReading;
			dialogueMesh.multSpeed = (Input.anyKey && isReading ? 20.0f : 1.0f);

			return isReading;
		}

		public void CallDialogue(string text)
		{
			dialoguePanel.SetActive(true);
			dialogueMesh.ReadText(text);
		}

		public void ClearDialogue()
		{
			dialoguePanel.SetActive(false);
			dialogueMesh.multSpeed = 1.0f;
		}

		public void UpdateAttackUI(LanderBattleHandler lander)
		{
            ClearAttackUI();
			attackPanel.SetActive(true);
			
            foreach (Move move in lander.Moves)
            {
                if (move == null)
                    continue;

                GameObject instance = Instantiate(attackUIPrefab, attackPanel.transform);
				AttackUIHandler attackUI = instance.GetComponent<AttackUIHandler>();
                attackUI.InitializeUI(move);
            }
		}

		public void ClearAttackUI()
		{
			attackPanel.SetActive(false);
            Transform attackTransform = attackPanel.transform;

            foreach (Transform child in attackTransform)
                Destroy(child.gameObject);
		}
	}
}
