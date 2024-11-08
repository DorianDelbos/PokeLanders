using System;
using System.Collections.Generic;
using System.Linq;

namespace LandersLegends.Extern.API
{
	public static class TypeRepository
	{
		private static Type[] typeList;
		private static Dictionary<(string, string), float> typeEfficiency = null;

		public static void Initialize()
		{
			typeList = DataFetcher<Type>.FetchArrayData($"api/v1/type");
			InitializeEfficiencyDictionary();
		}

		private static void InitializeEfficiencyDictionary()
		{
			typeEfficiency = new Dictionary<(string, string), float>();

			foreach (var type in typeList)
			{
				string typeFrom = type.name;

				foreach (var typeTo in type.damage_relations.double_damage_to)
					typeEfficiency.TryAdd((typeFrom.ToLower(), typeTo.ToLower()), 2.0f);

				foreach (var typeTo in type.damage_relations.half_damage_to)
					typeEfficiency.TryAdd((typeFrom.ToLower(), typeTo.ToLower()), 0.5f);
			}
		}

		public static Type[] GetAll() => typeList;
		public static Type GetByName(string name) => typeList.First(x => x.name == name);
		public static Type GetById(int id) => typeList.First(x => x.id == id);
		public static int GetIdByName(string name) => typeList.First(x => x.name == name).id;
		public static string GetNameById(int id) => typeList.First(x => x.id == id).name;
		public static bool IsExist(string name) => typeList.Any(x => x.name.Equals(name));
		public static float GetEfficiency(string attackerType, string defenderType)
		{
			attackerType = attackerType.ToLower();
			defenderType = defenderType.ToLower();

			if (!IsExist(attackerType))
				throw new Exception($"{attackerType} is not an existing type !");

			if (!IsExist(defenderType))
				throw new Exception($"{defenderType} is not an existing type !");

			if (typeEfficiency.TryGetValue((attackerType, defenderType), out float efficiency))
				return efficiency;

			return 1.0f; // Default value
		}
	}
}
