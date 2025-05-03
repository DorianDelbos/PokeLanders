using LandersLegends.Gameplay;
using UnityEngine;

namespace LandersLegends.Battle
{
	[System.Serializable]
	public class BattleStateMachine
    {
		[SerializeField] private BattleHUDHandler hudHandler;
		[SerializeField] private LanderBattleHandler[] landers;

		private BattleState currentState;
        private BattleStateFactory factory;

        public BattleStateFactory Factory => factory;
        public BattleHUDHandler HudHandler => hudHandler;
        public LanderBattleHandler[] Landers => landers;
        public LanderBattleHandler Lander1 => landers[0];
		public LanderBattleHandler Lander2 => landers[1];
        public LanderBattleHandler CurrentLander
        {
            get
			{
				if (currentState.GetType() == typeof(Player1State))
                    return landers[0];
				else if (currentState.GetType() == typeof(Player2State))
                    return landers[1];
                else
                    return null;
			}
        }

		public BattleStateMachine()
        {
            factory = new BattleStateFactory(this);
            factory.RegisterState(() => new StartState(this));
            factory.RegisterState(() => new Player1State(this));
            factory.RegisterState(() => new Player2State(this));
            factory.RegisterState(() => new AttackProcessState(this));
            factory.RegisterState(() => new EndState(this));
        }

        public void Start()
		{
			landers[0].InitializeLander(GameManager.instance.Landers[0]);
			landers[1].InitializeLander(GameManager.instance.Landers[1]);

			hudHandler.UpdateLandersHUD();
			ProcessState(factory.GetState<StartState>());
		}

        public void Update()
        {
            if (currentState != null)
                currentState.Update();
        }

        public void ProcessState(BattleState newState)
        {
            if (currentState != null)
                currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void ProcessNextState()
        {
            if (currentState.GetType() == typeof(Player1State))
                ProcessState(factory.GetState<Player2State>());
			else if (currentState.GetType() == typeof(Player2State))
				ProcessState(factory.GetState<AttackProcessState>());
			else if (currentState.GetType() == typeof(AttackProcessState))
				ProcessState(factory.GetState<Player1State>());
		}
    }
}
