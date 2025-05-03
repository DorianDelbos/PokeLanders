using LandersLegends.Extern.API;
using LandersLegends.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LandersLegends.Gameplay
{
	[Serializable]
	public class Lander
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
			public StatsData(int xp, ushort hp, byte happiness, string nature, Stats stats, Stats ivs, Stats evs)
			{
				this.xp = xp;
				this.hp = hp;
				this.happiness = happiness;
				this.nature = nature;
				this.stats = stats;
				this.ivs = ivs;
				this.evs = evs;
			}

			public int xp;
			public ushort hp;
			public byte happiness;
			public string nature;
			public Stats stats;
			public Stats ivs;
			public Stats evs;
		}

		[Serializable]
		public struct MovesData
		{
			public MovesData(ushort move1, ushort move2, ushort move3, ushort move4)
				: this(new ushort[] {move1,move2,move3,move4}) {}

			public MovesData(ushort[] moves)
			{
				this.moves = moves;
			}

			public ushort[] moves;
		}

		[Serializable]
		public struct OtherData
		{
			public OtherData(ushort baseXp, ushort height, ushort weight, bool isMale, bool isShiny, List<string> types, string modelUrl)
			{
				this.baseXp = baseXp;
				this.height = height;
				this.weight = weight;
				this.isMale = isMale;
				this.isShiny = isShiny;
				this.types = types;
				this.modelUrl = modelUrl;
			}

			public ushort baseXp;
			public ushort height;
			public ushort weight;
			public bool isMale;
			public bool isShiny;
			public List<string> types;
			public string modelUrl;
		}
		#endregion

		#region EVENTS
		public Action<int, int> onHpChange;
		#endregion

		#region ATTRIBUTS
		private MainData mainData;
		private StatsData statsData;
		private MovesData movesData;
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
		public bool IsMale => otherData.isMale;
		public bool IsShiny => otherData.isShiny;
		// Levels
		public byte Level => StatsCurves.GetLevelByXp(Xp, BaseXp);
		public int Xp { get => statsData.xp; private set => statsData.xp = value; }
		// States
		public ushort Hp { get => statsData.hp; set => statsData.hp = value; }
		public ushort MaxHp => StatsCurves.GetMaxHp(statsData.stats.hp, Level, statsData.ivs.hp, statsData.evs.hp);
		public ushort Attack => StatsCurves.GetStatValue(statsData.stats.attack, Level, statsData.ivs.attack, statsData.evs.attack, NatureRepository.GetByName(Nature).GetStatMultiplier("Attack"));
		public ushort SpecialAttack => StatsCurves.GetStatValue(statsData.stats.specialAttack, Level, statsData.ivs.specialAttack, statsData.evs.specialAttack, NatureRepository.GetByName(Nature).GetStatMultiplier("Special-Attack"));
		public ushort Defense => StatsCurves.GetStatValue(statsData.stats.defense, Level, statsData.ivs.defense, statsData.evs.defense, NatureRepository.GetByName(Nature).GetStatMultiplier("Defense"));
		public ushort SpecialDefense => StatsCurves.GetStatValue(statsData.stats.specialDefense, Level, statsData.ivs.specialDefense, statsData.evs.specialDefense, NatureRepository.GetByName(Nature).GetStatMultiplier("Special-Defense"));
		public ushort Speed => StatsCurves.GetStatValue(statsData.stats.speed, Level, statsData.ivs.speed, statsData.evs.speed, NatureRepository.GetByName(Nature).GetStatMultiplier("Speed"));
		public byte Happiness => statsData.happiness;
		public string Nature => statsData.nature;
		// Others
		public List<string> Types { get => otherData.types; private set => otherData.types = value; }
		public string ModelUrl { get => otherData.modelUrl; private set => otherData.modelUrl = value; }
		public ushort BaseXp { get => otherData.baseXp; private set => otherData.baseXp = value; }
		public ushort Height { get => otherData.height; private set => otherData.height = value; }
		public ushort Weight { get => otherData.weight; private set => otherData.weight = value; }
		// Attacks
		public ushort[] Moves => movesData.moves;
		#endregion

		#region CONSTRUCTORS
		public Lander(LanderDataNFC nfcData, Extern.API.Lander landerModel)
			: this(new MainData(nfcData.tag, nfcData.id, landerModel.name, nfcData.name, landerModel.description),
				  new StatsData(nfcData.xp, nfcData.hp, nfcData.happiness, NatureRepository.GetNameById(nfcData.nature), new Stats(landerModel.stats), new Stats(nfcData.ivPv, nfcData.ivAtk, nfcData.ivDef, nfcData.ivAtkSpe, nfcData.ivDefSpe, nfcData.ivSpeed), new Stats(nfcData.evPv, nfcData.evAtk, nfcData.evDef, nfcData.evAtkSpe, nfcData.evDefSpe, nfcData.evSpeed)),
				  new MovesData(nfcData.idAttack1, nfcData.idAttack2, nfcData.idAttack3, nfcData.idAttack4),
				  new OtherData(landerModel.base_experience, nfcData.height, nfcData.weight, nfcData.meta.GetBit(0), nfcData.meta.GetBit(1), landerModel.types, landerModel.model)) { }

		public Lander(string tag, ushort id, string species, string name, string description, int xp, ushort hp, byte happiness, string nature, byte baseHp, byte attack, byte specialAttack, byte defense, byte specialDefense, byte speed, byte ivPv, byte ivAtk, byte ivAtkSpe, byte ivDef, byte ivDefSpe, byte ivSpeed, byte evPv, byte evAtk, byte evAtkSpe, byte evDef, byte evDefSpe, byte evSpeed, ushort attack1, ushort attack2, ushort attack3, ushort attack4, ushort baseXp, ushort height, ushort weight, bool isMale, bool isShiny, List<string> types, string modelUrl)
			: this(new MainData(tag, id, species, name, description),
				   new StatsData(xp, hp, happiness, nature, new Stats(baseHp, attack, defense, specialAttack, specialDefense, speed), new Stats(ivPv, ivAtk, ivDef, ivAtkSpe, ivDefSpe, ivSpeed), new Stats(evPv, evAtk, evDef, evAtkSpe, evDefSpe, evSpeed)),
				   new MovesData(attack1, attack2, attack3, attack4),
				   new OtherData(baseXp, height, weight, isMale, isShiny, types, modelUrl)) { }

        public Lander(string tag, ushort id, string species, string name, string description, int xp, ushort hp, byte happiness, string nature, Stats stats, Stats ivs, Stats evs, ushort attack1, ushort attack2, ushort attack3, ushort attack4, ushort baseXp, ushort height, ushort weight, bool isMale, bool isShiny, List<string> types, string modelUrl)
			: this(new MainData(tag, id, species, name, description),
				   new StatsData(xp, hp, happiness, nature, stats, ivs, evs),
				   new MovesData(attack1, attack2, attack3, attack4),
				   new OtherData(baseXp, height, weight, isMale, isShiny, types, modelUrl)) { }

		public Lander(MainData mainData, StatsData statsData, MovesData attacksData, OtherData otherData)
		{
			this.mainData = mainData;
			this.statsData = statsData;
			this.movesData = attacksData;
			this.otherData = otherData;
		}
        #endregion

        #region METHODS
        public LanderDataNFC ToDataNFC() => new LanderDataNFC(mainData, statsData, movesData, otherData);

        public void TakeDamage(ushort damage)
		{
			Hp -= (ushort)Mathf.Min(damage, Hp);
			onHpChange?.Invoke(Hp, MaxHp);
		}
		#endregion
	}
}
