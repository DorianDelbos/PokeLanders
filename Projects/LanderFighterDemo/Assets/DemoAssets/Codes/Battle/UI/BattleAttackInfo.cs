using dgames.Utils;
using Landers;
using Landers.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LanderFighter
{
    public class BattleAttackInfo : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text nameMesh;
        [SerializeField] private TMP_Text remaningMesh;

        private LanderData m_attacker;
        private Move m_move;

        public void InitializeData(LanderData data, Move move)
        {
            m_attacker = data;
            m_move = move;
            Type moveType = TypeRepository.Instance.GetByName(move.type);

            background.color = moveType.color.ToColor();
            nameMesh.text = move.name;
            remaningMesh.text = $"??/{move.pp}";
        }

        public void OnClick()
        {
            BattleSystem.Current.ProcessAttack(m_attacker, m_move);
        }
    }
}
