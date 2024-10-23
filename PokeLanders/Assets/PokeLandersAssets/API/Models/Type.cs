namespace Lander.API
{
	[System.Serializable]
	public class DamageRelations
	{
		public string[] doubleDamageFrom;
		public string[] doubleDamageTo;
		public string[] halfDamageFrom;
		public string[] halfDamageTo;
		public string[] noneDamageFrom;
		public string[] noneDamageTo;
	}

	[System.Serializable]
	public class Type : IBaseModel
	{
		public int id;
		public string name;
		public DamageRelations damageRelations;
	}
}
