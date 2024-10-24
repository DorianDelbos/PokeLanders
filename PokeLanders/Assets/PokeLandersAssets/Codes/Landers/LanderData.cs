using Lander.Extern;
using Lander.Gameplay.Type;
using Lander.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lander.Gameplay
{
	[Serializable]
	public class LanderData
	{
		#region ATTRIBUTS
		// Main
		private string tag = string.Empty;
		private int id = 0;
		private string species = string.Empty;
		private string name = string.Empty;
		private string description = string.Empty;
		// Stats
		private int level = 0;
		private int xp = 0;
		private int hp = 0;
		private LanderStats stats;
		// Others
		private int height = 0;
		private int weight = 0;
		private List<ElementaryType> types = null;
		private Mesh mesh = null;
		#endregion

		#region EVENTS
		public Action<int, int> onHpChange;
		#endregion

		#region GETTERS/SETTERS
		// Main
		public string Tag => tag;
		public int ID => id;
		public string Species => species;
		public string Name => name;
		public string Description => description;
		// Levels
		public int Level => level;
		public int Xp => xp;
		// States
		public int Hp => hp;
		public int MaxHp => StatsCurves.GetMaxHp(stats.baseHp, level);
		public int BaseHp => stats.baseHp;
		public int Attack => stats.attack;
		public int SpecialAttack => stats.specialAttack;
		public int Defense => stats.defense;
		public int SpecialDefense => stats.specialDefense;
		public int Speed => stats.speed;
		// Others
		public List<ElementaryType> Types => types;
		public Mesh Mesh => mesh;
		public int Height => height;
		public int Weight => weight;
		#endregion

		#region CONSTRUCTORS
		public LanderData(LanderDataNFC nfcData, Extern.API.Lander landerModel)
			=> SetLanderBaseData(nfcData.tag, nfcData.id, landerModel.name, nfcData.name, landerModel.description, nfcData.level, nfcData.xp, nfcData.hp, new LanderStats(landerModel.stats), nfcData.height, nfcData.weight, ElementaryTypeUtils.StringsToTypes(landerModel.types), null /*todo*/);

		public LanderData(string tag, int id, string species, string name, string description, int level, int xp, int hp, int baseHp, int attack, int specialAttack, int defense, int specialDefense, int speed, int height, int weight, List<ElementaryType> types, Mesh mesh)
			=> SetLanderBaseData(tag, id, species, name, description, level, xp, hp, new LanderStats(baseHp, attack, defense, specialAttack, specialDefense, speed), height, weight, types, mesh);

		public LanderData(string tag, int id, string species, string name, string description, int level, int xp, int hp, LanderStats stats, int height, int weight, List<ElementaryType> types, Mesh mesh)
	=> SetLanderBaseData(tag, id, species, name, description, level, xp, hp, stats, height, weight, types, mesh);
		#endregion

		#region STATICS
		public static LanderData CreateRandomLander() => CreateRandomLander(UnityEngine.Random.Range(1, 100));

		public static LanderData CreateRandomLander(int level)
		{
			Extern.API.Lander[] allLanders = APIDataFetcher<Extern.API.Lander>.FetchArrayData("api/v1/lander");
			Extern.API.Lander landerModel = allLanders.Where(x => x.id == UnityEngine.Random.Range(0, allLanders.Length)).First();
			int maxHp = StatsCurves.GetMaxHp(landerModel.stats.Where(x => x.stat == "pv").First().base_stat, level);

			return new LanderData(
				"-1",
				landerModel.id,
				landerModel.name,
				landerModel.name,
				landerModel.description,
				level,
				landerModel.base_experience,
				maxHp,
				new LanderStats(landerModel.stats),
				landerModel.base_height,
				landerModel.base_weight,
				ElementaryTypeUtils.StringsToTypes(landerModel.types),
				null // TODO
			);
		}
		#endregion

		#region METHODS
		private void SetLanderBaseData(string tag, int id, string species, string name, string description, int level, int xp, int hp, LanderStats stats, int height, int weight, List<ElementaryType> types, Mesh mesh)
		{
			this.tag = tag;
			this.id = id;
			this.species = species;
			this.name = name;
			this.description = description;
			this.level = level;
			this.xp = xp;
			this.hp = hp;
			this.stats = stats;
			this.height = height;
			this.weight = weight;
			this.types = types;
			this.mesh = mesh;
		}

		public void TakeDamage(int damage)
		{
			hp -= Mathf.Min(damage, 255);
			onHpChange?.Invoke(Hp, MaxHp);
		}
		#endregion
	}
}
