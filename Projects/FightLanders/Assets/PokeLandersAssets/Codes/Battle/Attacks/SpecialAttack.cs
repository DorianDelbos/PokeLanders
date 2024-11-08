using UnityEngine;

namespace LandersLegends.Gameplay.Attack
{
	public class SpecialAttack : Attack
	{
		public override void Use(Lander attacker, Lander defenser)
		{
			defenser.TakeDamage(CalculDamage(attacker, defenser));
		}

		protected override ushort CalculDamage(Lander attacker, Lander defenser)
		{
			return (ushort)Mathf.FloorToInt(((2 * attacker.Level + 10) / 250 * attacker.SpecialAttack / defenser.SpecialDefense * Power + 2) * Random.Range(0.85f, 1.0f));
		}
	}
}
