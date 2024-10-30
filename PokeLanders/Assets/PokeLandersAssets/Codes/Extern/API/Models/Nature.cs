using System.Collections.Generic;

namespace LandersLegends.Extern.API
{
	[System.Serializable]
	public class StatChanges
	{
		public int change;
		public string stat;
	}

	[System.Serializable]
	public class Nature : IBaseModel
	{
		public int id;
		public string name;
		public List<StatChanges> stat_changes;
	}
}
