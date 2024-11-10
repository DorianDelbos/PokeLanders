using System.Collections.Generic;
using UnityEngine;

namespace Dgames.Extern.API
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

        public float GetStatMultiplier(string stat)
        {
            stat = stat.ToLower();
            if (!StatRepository.IsExist(stat))
                Debug.LogError($"The stat \"{stat}\" don't exist in this context !");

            foreach (var statChange in stat_changes)
            {
                if (statChange.stat == stat)
                {
                    return statChange.change switch
                    {
                        -1 => 0.9f,
                        1 => 1.1f,
                        _ => 1.0f
                    };
                }
            }

            return 1.0f;
        }
    }
}
