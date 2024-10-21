public abstract class Attack
{
	private ushort id;
	private string name;
	private ElementaryType type;
	private ushort power;
	private ushort precision;
	private ushort pp;

	public ushort ID => id;
	public string Name => name;
	public ElementaryType Type => type;
	public ushort Power => power;
	public ushort Precision => precision;
	public ushort Pp => pp;

	public abstract void Use(LanderData attacker, LanderData defenser);
	protected abstract int CalculDamage(LanderData attacker, LanderData defenser);
}
