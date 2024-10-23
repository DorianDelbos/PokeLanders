using System.Collections.Generic;

namespace Lander.Gameplay.Type
{
	public static class TypeEfficiency
	{
		// Dictionary that associates each type with its effectiveness against other types
		// First type -> attacker / Second type -> defender
		private static Dictionary<(ElementaryType, ElementaryType), float> dictEfficiency = new Dictionary<(ElementaryType, ElementaryType), float>
	{
		{(ElementaryType.Fire,  ElementaryType.Water),  0.5f},
		{(ElementaryType.Fire,  ElementaryType.Grass),  2.0f},
		{(ElementaryType.Water, ElementaryType.Fire),   2.0f},
		{(ElementaryType.Water, ElementaryType.Grass),  0.5f},
		{(ElementaryType.Grass, ElementaryType.Water),  2.0f},
		{(ElementaryType.Grass, ElementaryType.Fire),   0.5f},
		{(ElementaryType.Light, ElementaryType.Dark),   2.0f},
		{(ElementaryType.Dark,  ElementaryType.Light),  2.0f},
	};

		public static float GetEfficiency(ElementaryType attackerType, ElementaryType defenderType)
		{
			if (dictEfficiency.TryGetValue((attackerType, defenderType), out float efficiency))
				return efficiency;

			return 1.0f; // Default value
		}
	}
}
