namespace LandersLegends.Gameplay.Attack
{
	public abstract class Attack
	{
		private ushort id;
		private string name;
		private string type;
		private ushort power;
		private ushort precision;
		private ushort pp;

		public ushort ID => id;
		public string Name => name;
		public string Type => type;
		public ushort Power => power;
		public ushort Precision => precision;
		public ushort Pp => pp;

		public abstract void Use(Lander attacker, Lander defenser);
		protected abstract ushort CalculDamage(Lander attacker, Lander defenser);
	}
}
