using UnityEngine;

namespace Lander.Gameplay.Attack
{
	public class PhysicalAttack : Attack
	{
		public override void Use(LanderData attacker, LanderData defenser)
		{
			throw new System.NotImplementedException();
		}

		protected override ushort CalculDamage(LanderData attacker, LanderData defenser)
		{
			return (ushort)Mathf.FloorToInt(((2 * attacker.Level + 10) / 250 * attacker.Attack / defenser.Defense * Power + 2) * Random.Range(0.85f, 1.0f));
		}
	}
}
