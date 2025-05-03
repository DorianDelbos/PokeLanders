using LandersLegends.Extern.API;
using System.Linq;
using UnityEngine;

namespace LandersLegends.Battle
{
	[System.Serializable]
	public class LanderBattleHandler
	{
		public void InitializeLander(Gameplay.Lander lander)
		{
			this.lander = lander;
			moves = MoveRepository.GetAll()
				.Where(x => lander.Moves.Contains((ushort)x.id))
				.Select(x => x.Clone() as Move)
				.ToArray();
		}

		private Gameplay.Lander lander;
		private Move[] moves = new Move[4];
		[SerializeField] private BattleCinematicLanderHandler cinematicHandler;
		[SerializeField] private bool isAi = false;

		[HideInInspector] public ushort lastMoveProccess = 0;

		public Gameplay.Lander Lander => lander;
		public Move[] Moves => moves;
		public BattleCinematicLanderHandler CinematicHandler => cinematicHandler;
		public bool IsAi => isAi;
	}
}
