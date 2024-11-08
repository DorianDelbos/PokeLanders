using LandersLegends.Gameplay;
using TMPro;
using UnityEngine;

namespace LandersLegends.Battle
{
    public class BattleHUDHandler : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private GameObject attackPanel;
        [SerializeField] private BattleLanderHudHandler[] hudHandler = new BattleLanderHudHandler[2];
        public TMP_Dialogue dialogueMesh;

        private Lander[] landers => GameManager.instance.Landers;

        public void UpdateLandersHUD()
        {
            for (int i = 0; i < landers.Length; i++)
            {
                hudHandler[i].UpdateData(landers[i]);
            }
        }

        public void SetActiveDialogue(bool active)
        {
            dialoguePanel.SetActive(active);
        }

        public void SetActiveAttack(bool active)
        {
            attackPanel.SetActive(active);
        }
    }
}
