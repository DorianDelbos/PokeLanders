using UnityEngine;

namespace Lander.Gameplay.Attack
{
	public class SpecialAttack : Attack
	{
		public override void Use(LanderData attacker, LanderData defenser)
		{
			defenser.TakeDamage(CalculDamage(attacker, defenser));
		}

		protected override ushort CalculDamage(LanderData attacker, LanderData defenser)
		{
			return (ushort)Mathf.FloorToInt(((2 * attacker.Level + 10) / 250 * attacker.SpecialAttack / defenser.SpecialDefense * Power + 2) * Random.Range(0.85f, 1.0f));
		}
	}
}
