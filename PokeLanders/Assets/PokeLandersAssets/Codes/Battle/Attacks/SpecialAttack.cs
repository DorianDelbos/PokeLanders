using UnityEngine;

public class SpecialAttack : Attack
{
	public override void Use(LanderData attacker, LanderData defenser)
	{
		defenser.TakeDamage(CalculDamage(attacker, defenser));
	}

	protected override int CalculDamage(LanderData attacker, LanderData defenser)
	{
		return Mathf.FloorToInt(((2 * attacker.Level + 10) / 250 * attacker.SpecialAttack / defenser.SpecialDefense * Power + 2) * Random.Range(0.85f, 1.0f));
	}
}
