using GLTFast;
using Landers;
using Landers.API;
using Landers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LanderFighter
{
    public class BattleSystem : MonoBehaviour
    {
        public static BattleSystem Current;

        public LanderData LanderPlayer { get; private set; }
        public LanderData LanderOpponent { get; private set; }

        public BattleTextInfo BattleTextInfo => battleTextInfo;
        public BattleAttackListInfo BattleAttackListInfo => battleAttackListInfo;

        [SerializeField] private BattleTextInfo battleTextInfo;
        [SerializeField] private BattleAttackListInfo battleAttackListInfo;

        [SerializeField] private GltfAsset playerGltf;
        [SerializeField] private GltfAsset opponentGltf;

        [SerializeField] private BattleInfo playerBattleInfo;
        [SerializeField] private BattleInfo opponentBattleInfo;

        private Dictionary<BattleState, BattleStateBase> BattleStates;
        public BattleStateBase CurrentState;

        public Dictionary<string, Move> AttacksRegister = new Dictionary<string, Move>();

        public static event Action<BattleState> OnBattleStateChanged;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            InitializeLander(UserLanderManager.Instance.UserLanderData, LanderUtils.RandomLander(UserLanderManager.Instance.UserLanderData.Level, 5));
            InitializeBattleStates();
            ProcessState(BattleState.BeginBattle);
        }

        private void Update()
        {
            UpdateState();
        }

        private void InitializeBattleStates()
        {
            BattleStates = new Dictionary<BattleState, BattleStateBase>()
            {
                { BattleState.BeginBattle, new BeginBattleState() },
                { BattleState.PlayersChoice, new PlayersChoiceBattleState() },
                { BattleState.ProcessAttacks, new ProcessAttacksBattleState() },
                { BattleState.EndBattle, new EndBattleState() },
                { BattleState.ReturnMainMenu, new ReturnMainMenuState() },
            };
        }

        private void InitializeLander(LanderData player, LanderData opponent)
        {
            LanderPlayer = player;
            LanderOpponent = opponent;

            TaskHandler.Instance.LoadModel3D(playerGltf, LanderPlayer.ModelUrl);
            TaskHandler.Instance.LoadModel3D(opponentGltf, LanderOpponent.ModelUrl);

            playerBattleInfo.IntializeData(LanderPlayer);
            opponentBattleInfo.IntializeData(LanderOpponent);
        }

        private void ProcessState(BattleState state)
        {
            if (CurrentState != null)
                CurrentState.OnExit();
            CurrentState = BattleStates[state];
            CurrentState.OnEnter();

            // Events
            OnBattleStateChanged?.Invoke(state);
        }

        private void UpdateState()
        {
            if (CurrentState == null) return;

            BattleStateResult result = CurrentState.OnUpdate();

            switch (result)
            {
                case BattleStateResult.NextState:
                    ProcessState(CurrentState.GetNextState());
                    break;
                case BattleStateResult.Running:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void ProcessAttack(LanderData attacker, Move move)
        {
            if (!AttacksRegister.TryAdd(attacker.Tag, move))
            {
                AttacksRegister[attacker.Tag] = move;
            }
        }
    }
}
