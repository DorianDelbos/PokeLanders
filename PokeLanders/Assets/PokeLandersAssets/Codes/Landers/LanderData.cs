using Lander.Extern;
using Lander.Gameplay.Type;
using Lander.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lander.Gameplay
{
	using BundleAssetsLoad = Dictionary<System.Type, List<UnityEngine.Object>>;

	[Serializable]
	public class LanderData
	{
		#region STRUCT
		[Serializable]
		public struct Stats
		{
			public Stats(byte hp, byte attack, byte defense, byte specialAttack, byte specialDefense, byte speed)
			{
				this.hp = hp;
				this.attack = attack;
				this.defense = defense;
				this.specialAttack = specialAttack;
				this.specialDefense = specialDefense;
				this.speed = speed;
			}

			public Stats(List<Extern.API.Stats> stats)
			{
				hp = stats.Where(x => x.stat == "pv").First().base_stat;
				attack = stats.Where(x => x.stat == "attack").First().base_stat;
				defense = stats.Where(x => x.stat == "defense").First().base_stat;
				specialAttack = stats.Where(x => x.stat == "special-attack").First().base_stat;
				specialDefense = stats.Where(x => x.stat == "special-defense").First().base_stat;
				speed = stats.Where(x => x.stat == "speed").First().base_stat;
			}

			public byte hp;
			public byte attack;
			public byte defense;
			public byte specialAttack;
			public byte specialDefense;
			public byte speed;
		}

		[Serializable]
		public struct MainData
		{
			public MainData(string tag, ushort id, string species, string name, string description)
			{
				this.tag = tag;
				this.id = id;
				this.species = species;
				this.name = name;
				this.description = description;
			}

			public string tag;
			public ushort id;
			public string species;
			public string name;
			public string description;
		}

		[Serializable]
		public struct StatsData
		{
			public StatsData(int xp, ushort hp, Stats stats, Stats ivs, Stats evs)
			{
				this.xp = xp;
				this.hp = hp;
				this.stats = stats;
				this.ivs = ivs;
				this.evs = evs;
			}

			public int xp;
			public ushort hp;
			public Stats stats;
			public Stats ivs;
			public Stats evs;
		}

		[Serializable]
		public struct OtherData
		{
			public OtherData(ushort baseXp, ushort height, ushort weight, byte meta, List<ElementaryType> types, BundleAssetsLoad bundleModel)
			{
				this.baseXp = baseXp;
				this.height = height;
				this.weight = weight;
				this.meta = meta;
				this.types = types;
				this.bundleModel = bundleModel;
			}

			public ushort baseXp;
			public ushort height;
			public ushort weight;
			public byte meta;
			public List<ElementaryType> types;
			public BundleAssetsLoad bundleModel;
		}
		#endregion

		#region EVENTS
		public Action<int, int> onHpChange;
		#endregion

		#region ATTRIBUTS
		private MainData mainData;
		private StatsData statsData;
		private OtherData otherData;
		#endregion

		#region GETTERS/SETTERS
		// Main
		public string Tag { get => mainData.tag; private set => mainData.tag = value; }
		public ushort ID { get => mainData.id; private set => mainData.id = value; }
		public string Species { get => mainData.species; private set => mainData.species = value; }
		public string Name { get => mainData.name; private set => mainData.name = value; }
		public string Description { get => mainData.description; private set => mainData.description = value; }
		// Meta
		public bool IsMale => otherData.meta.GetBit(0);
		public bool IsShiny => otherData.meta.GetBit(1);
        // Levels
        public byte Level => StatsCurves.GetLevelByXp(Xp, BaseXp);
		public int Xp { get => statsData.xp; private set => statsData.xp = value; }
        // States
        public ushort Hp { get => statsData.hp; set => statsData.hp = value; }
		public ushort MaxHp => StatsCurves.GetMaxHp(statsData.stats.hp, Level, statsData.ivs.hp, statsData.evs.hp);
		public ushort Attack => StatsCurves.GetStatValue(Level, statsData.ivs.attack, statsData.evs.attack, statsData.stats.attack);
		public ushort SpecialAttack => StatsCurves.GetStatValue(Level, statsData.ivs.specialAttack, statsData.evs.specialAttack, statsData.stats.specialAttack);
		public ushort Defense => StatsCurves.GetStatValue(Level, statsData.ivs.defense, statsData.evs.defense, statsData.stats.defense);
        public ushort SpecialDefense => StatsCurves.GetStatValue(Level, statsData.ivs.specialDefense, statsData.evs.specialDefense, statsData.stats.specialDefense);
        public ushort Speed => StatsCurves.GetStatValue(Level, statsData.ivs.speed, statsData.evs.speed, statsData.stats.speed);
        // Others
        public List<ElementaryType> Types { get => otherData.types; private set => otherData.types = value; }
		public BundleAssetsLoad BundleModel { get => otherData.bundleModel; private set => otherData.bundleModel = value; }
        public ushort BaseXp { get => otherData.baseXp; private set => otherData.baseXp = value; }
        public ushort Height { get => otherData.height; private set => otherData.height = value; }
		public ushort Weight { get => otherData.weight; private set => otherData.weight = value; }
		#endregion

		#region CONSTRUCTORS
		public LanderData(LanderDataNFC nfcData, Extern.API.Lander landerModel)
			=> SetLanderBaseData(new MainData(nfcData.tag, nfcData.id, landerModel.name, nfcData.name, landerModel.description), new StatsData(nfcData.xp, nfcData.hp, new Stats(landerModel.stats), new Stats(nfcData.ivPv, nfcData.ivAtk, nfcData.ivDef, nfcData.ivAtkSpe, nfcData.ivDefSpe, nfcData.ivSpeed), new Stats(nfcData.evPv, nfcData.evAtk, nfcData.evDef, nfcData.evAtkSpe, nfcData.evDefSpe, nfcData.evSpeed)), new OtherData(landerModel.base_experience, nfcData.height, nfcData.weight, nfcData.meta, ElementaryTypeUtils.StringsToTypes(landerModel.types), BundleLoaderUtils.DownloadAssets(landerModel.bundle)));

		public LanderData(string tag, ushort id, string species, string name, string description, int xp, ushort hp, byte baseHp, byte attack, byte specialAttack, byte defense, byte specialDefense, byte speed, byte ivPv, byte ivAtk, byte ivAtkSpe, byte ivDef, byte ivDefSpe, byte ivSpeed, byte evPv, byte evAtk, byte evAtkSpe, byte evDef, byte evDefSpe, byte evSpeed, ushort baseXp, ushort height, ushort weight, byte meta, List<ElementaryType> types, BundleAssetsLoad bundleModel)
			=> SetLanderBaseData(new MainData(tag, id, species, name, description), new StatsData(xp, hp, new Stats(baseHp, attack, defense, specialAttack, specialDefense, speed), new Stats(ivPv, ivAtk, ivDef, ivAtkSpe, ivDefSpe, ivSpeed), new Stats(evPv, evAtk, evDef, evAtkSpe, evDefSpe, evSpeed)), new OtherData(baseXp, height, weight, meta, types, bundleModel));

        public LanderData(string tag, ushort id, string species, string name, string description, int xp, ushort hp, Stats stats, Stats ivs, Stats evs, ushort baseXp, ushort height, ushort weight, byte meta, List<ElementaryType> types, BundleAssetsLoad bundleModel)
            => SetLanderBaseData(new MainData(tag, id, species, name, description), new StatsData(xp, hp, stats, ivs, evs), new OtherData(baseXp, height, weight, meta, types, bundleModel));

        public LanderData(MainData mainData, StatsData statsData, OtherData otherData)
            => SetLanderBaseData(mainData, statsData, otherData);
        #endregion

        #region METHODS
        private void SetLanderBaseData(MainData mainData, StatsData statsData, OtherData otherData)
		{
			this.mainData = mainData;
			this.statsData = statsData;
			this.otherData = otherData;

			_ = IsMale;
			_ = IsShiny;
		}

		public void TakeDamage(ushort damage)
		{
			Hp -= (ushort)Mathf.Min(damage, MaxHp);
			onHpChange?.Invoke(Hp, MaxHp);
		}
		#endregion
	}
}
