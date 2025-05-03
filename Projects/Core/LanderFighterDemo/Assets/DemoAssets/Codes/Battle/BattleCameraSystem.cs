using Unity.Cinemachine;
using UnityEngine;

namespace LanderFighter
{
    public class BattleCameraSystem : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera MainCamera;
        [SerializeField] private CinemachineCamera PlayerCamera;
        [SerializeField] private CinemachineCamera OpponentCamera;

        private void OnEnable()
        {
            BattleSystem.OnBattleStateChanged += HandleBattleStateChanged;
        }

        private void OnDisable()
        {
            BattleSystem.OnBattleStateChanged -= HandleBattleStateChanged;
        }

        private void HandleBattleStateChanged(BattleState state)
        {
            switch (state)
            {
                case BattleState.BeginBattle:
                case BattleState.ProcessAttacks:
                    MainCamera.Prioritize();
                    break;
                case BattleState.PlayersChoice:
                    PlayerCamera.Prioritize();
                    break;
                default:
                    break;
            }
        }
    }
}
