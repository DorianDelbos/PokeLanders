using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LandersLegends.Battle
{
    public class BattleStateMachine : MonoBehaviour
    {
        private BattleState currentState;
        private BattleHUDHandler hudHandler;
        private BattleStateFactory factory;
        [SerializeField] private BattleLanderHandler[] battleLandersHandler = new BattleLanderHandler[2];

        public BattleStateFactory Factory => factory;
        public BattleHUDHandler HudHandler => hudHandler;
        public BattleLanderHandler[] BattleLandersHandler => battleLandersHandler;

        private void Start()
        {
            factory = new BattleStateFactory(this);

            hudHandler = FindFirstObjectByType<BattleHUDHandler>();
            hudHandler.UpdateLandersHUD();

            ProcessState(factory.GetState<StartState>());
        }

        private void Update()
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
    }
    public class BattleStateFactory
    {
        private readonly BattleStateMachine _stateMachine;
        private readonly Dictionary<Type, Func<BattleState>> _stateCreators;

        public BattleStateFactory(BattleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _stateCreators = new Dictionary<Type, Func<BattleState>>();

            RegisterStates();
        }

        private void RegisterStates()
        {
            // Get all types that inherit from BattleState
            var stateTypes = Assembly.GetAssembly(typeof(BattleState))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BattleState)) && !t.IsAbstract);

            foreach (var stateType in stateTypes)
            {
                // Create a delegate to instantiate the state
                var constructor = stateType.GetConstructor(new[] { typeof(BattleStateMachine) });
                if (constructor != null)
                {
                    _stateCreators[stateType] = () => (BattleState)constructor.Invoke(new object[] { _stateMachine });
                }
            }
        }

        public T GetState<T>() where T : BattleState
        {
            if (_stateCreators.TryGetValue(typeof(T), out var creator))
            {
                return (T)creator();
            }
            throw new InvalidOperationException($"State of type {typeof(T).Name} not registered in factory.");
        }
    }
}
