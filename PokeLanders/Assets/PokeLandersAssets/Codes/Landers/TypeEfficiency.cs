using LandersLegends.Extern;
using System.Collections.Generic;

namespace LandersLegends.Gameplay.Type
{
	public static class TypeEfficiency
	{
		// Dictionary that associates each type with its effectiveness against other types
		// First type -> attacker / Second type -> defender
		private static Dictionary<(ElementaryType, ElementaryType), float> typeEfficiency = null;

		private static void MakeEfficiencyDictionary()
		{
			typeEfficiency = new Dictionary<(ElementaryType, ElementaryType), float>();
			Extern.API.Type[] types = APIDataFetcher<Extern.API.Type>.FetchArrayData($"api/v1/type");

			foreach (var type in types)
			{
				ElementaryType from = ElementaryTypeUtils.StringToType(type.name);

				foreach (var typeTo in type.damage_relations.double_damage_to)
				{
					ElementaryType typeToS = ElementaryTypeUtils.StringToType(type.name);
					typeEfficiency.TryAdd((from, typeToS), 2.0f);
				}

				foreach (var typeTo in type.damage_relations.half_damage_to)
				{
					ElementaryType typeToS = ElementaryTypeUtils.StringToType(type.name);
					typeEfficiency.TryAdd((from, typeToS), 0.5f);
				}
			}
		}

		public static float GetEfficiency(ElementaryType attackerType, ElementaryType defenderType)
		{
			if (typeEfficiency == null)
				MakeEfficiencyDictionary();

			if (typeEfficiency.TryGetValue((attackerType, defenderType), out float efficiency))
				return efficiency;

			return 1.0f; // Default value
		}
	}
}
