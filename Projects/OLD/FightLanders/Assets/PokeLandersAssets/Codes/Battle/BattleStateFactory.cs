using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LandersLegends.Battle
{
	public class BattleStateFactory
	{
		private readonly BattleStateMachine _stateMachine;
		private readonly Dictionary<Type, Func<BattleState>> _stateCreators;
		private readonly Dictionary<Type, BattleState> _cachedStates; // Optional for reuse

		public BattleStateFactory(BattleStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
			_stateCreators = new Dictionary<Type, Func<BattleState>>();
			_cachedStates = new Dictionary<Type, BattleState>();
		}

		/// <summary>
		/// Registers a state type with a factory method.
		/// </summary>
		public void RegisterState<T>(Func<T> creator) where T : BattleState
		{
			var type = typeof(T);
			if (_stateCreators.ContainsKey(type))
				throw new InvalidOperationException($"State {type.Name} is already registered.");

			_stateCreators[type] = () => creator();
		}

		/// <summary>
		/// Gets or creates a state of the specified type.
		/// </summary>
		public T GetState<T>() where T : BattleState
		{
			var type = typeof(T);

			// Check for cached state
			if (_cachedStates.TryGetValue(type, out var cachedState))
				return (T)cachedState;

			// Create new state
			if (_stateCreators.TryGetValue(type, out var creator))
			{
				var state = (T)creator();
				_cachedStates[type] = state; // Cache the state (optional)
				return state;
			}

			throw new InvalidOperationException($"State of type {type.Name} is not registered.");
		}

		/// <summary>
		/// Tries to get or create a state of the specified type.
		/// </summary>
		public bool TryGetState<T>(out T state) where T : BattleState
		{
			state = null;

			var type = typeof(T);

			// Check for cached state
			if (_cachedStates.TryGetValue(type, out var cachedState))
			{
				state = (T)cachedState;
				return true;
			}

			// Try to create new state
			if (_stateCreators.TryGetValue(type, out var creator))
			{
				state = (T)creator();
				_cachedStates[type] = state; // Cache the state (optional)
				return true;
			}

			return false;
		}

		/// <summary>
		/// Clears cached states to allow fresh creation.
		/// </summary>
		public void ClearCache()
		{
			_cachedStates.Clear();
		}
	}
}
