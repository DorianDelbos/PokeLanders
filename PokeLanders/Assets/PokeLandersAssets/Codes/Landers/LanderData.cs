using Lander.Extern;
using Lander.Gameplay.Type;
using Lander.Maths;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lander.Gameplay
{
	using BundleAssetsLoad = Dictionary<System.Type, List<UnityEngine.Object>>;

	[Serializable]
	public class LanderData
	{
		#region EVENTS
		public Action<int, int> onHpChange;
		#endregion

		#region ATTRIBUTS
		// Main
		private string tag = string.Empty;
		private ushort id = 0;
		private string species = string.Empty;
		private string name = string.Empty;
		private string description = string.Empty;
		// Stats
		private int xp = 0;
		private ushort hp = 0;
        private LanderStats stats;
		private LanderStats ivs;
		private LanderStats evs;
        // Others
        private ushort baseXp = 0;
        private ushort height = 0;
		private ushort weight = 0;
		private List<ElementaryType> types = null;
		private BundleAssetsLoad bundleModel = null;
		#endregion

		#region GETTERS/SETTERS
		// Main
		public string Tag => tag;
		public ushort ID => id;
		public string Species => species;
		public string Name => name;
		public string Description => description;
		// Levels
		public byte Level => StatsCurves.GetLevelByXp(xp, baseXp);
		public int Xp => xp;
        // States
        public ushort Hp => hp;
		public ushort MaxHp => StatsCurves.GetMaxHp(stats.hp, Level, IvPv, EvPv);
		public byte BaseHp => stats.hp;
		public byte Attack => stats.attack;
		public byte SpecialAttack => stats.specialAttack;
		public byte Defense => stats.defense;
		public byte SpecialDefense => stats.specialDefense;
		public byte Speed => stats.speed;
        // IVs
        public byte IvPv => ivs.hp;
        public byte IvAtk => ivs.attack;
        public byte IvDef => ivs.defense;
        public byte IvAtkSpe => ivs.specialAttack;
        public byte IvDefSpe => ivs.specialDefense;
        public byte IvSpeed => ivs.speed;
        // EVs
        public byte EvPv => evs.hp;
        public byte EvAtk => evs.attack;
        public byte EvDef => evs.defense;
        public byte EvAtkSpe => evs.specialAttack;
        public byte EvDefSpe => evs.specialDefense;
        public byte EvSpeed => evs.speed;
        // Others
        public List<ElementaryType> Types => types;
		public BundleAssetsLoad BundleModel => bundleModel;
        public ushort BaseXp => baseXp;
        public ushort Height => height;
		public ushort Weight => weight;
		#endregion

		#region CONSTRUCTORS
		public LanderData(LanderDataNFC nfcData, Extern.API.Lander landerModel)
			=> SetLanderBaseData(nfcData.tag, nfcData.id, landerModel.name, nfcData.name, landerModel.description, nfcData.xp, nfcData.hp, new LanderStats(landerModel.stats), new LanderStats(nfcData.ivPv, nfcData.ivAtk, nfcData.ivDef, nfcData.ivAtkSpe, nfcData.ivDefSpe, nfcData.ivSpeed), new LanderStats(nfcData.evPv, nfcData.evAtk, nfcData.evDef, nfcData.evAtkSpe, nfcData.evDefSpe, nfcData.evSpeed), landerModel.base_experience, nfcData.height, nfcData.weight, ElementaryTypeUtils.StringsToTypes(landerModel.types), BundleLoaderUtils.DownloadAssets(landerModel.bundle));

		public LanderData(string tag, ushort id, string species, string name, string description, int xp, ushort hp, byte baseHp, byte attack, byte specialAttack, byte defense, byte specialDefense, byte speed, byte ivPv, byte ivAtk, byte ivAtkSpe, byte ivDef, byte ivDefSpe, byte ivSpeed, byte evPv, byte evAtk, byte evAtkSpe, byte evDef, byte evDefSpe, byte evSpeed, ushort baseXp, ushort height, ushort weight, List<ElementaryType> types, BundleAssetsLoad bundleModel)
			=> SetLanderBaseData(tag, id, species, name, description, xp, hp, new LanderStats(baseHp, attack, defense, specialAttack, specialDefense, speed), new LanderStats(ivPv, ivAtk, ivDef, ivAtkSpe, ivDefSpe, ivSpeed), new LanderStats(evPv, evAtk, evDef, evAtkSpe, evDefSpe, evSpeed), baseXp, height, weight, types, bundleModel);

		public LanderData(string tag, ushort id, string species, string name, string description, int xp, ushort hp, LanderStats stats, LanderStats ivs, LanderStats evs, ushort baseXp, ushort height, ushort weight, List<ElementaryType> types, BundleAssetsLoad bundleModel)
			=> SetLanderBaseData(tag, id, species, name, description, xp, hp, stats, ivs, evs, baseXp, height, weight, types, bundleModel);
		#endregion

		#region METHODS
		private void SetLanderBaseData(string tag, ushort id, string species, string name, string description, int xp, ushort hp, LanderStats stats, LanderStats ivs, LanderStats evs, ushort baseXp, ushort height, ushort weight, List<ElementaryType> types, BundleAssetsLoad bundleModel)
		{
			this.tag = tag;
			this.id = id;
			this.species = species;
			this.name = name;
			this.description = description;
			this.xp = xp;
			this.hp = hp;
			this.stats = stats;
			this.ivs = ivs;
			this.evs = evs;
			this.baseXp = baseXp;
			this.height = height;
			this.weight = weight;
			this.types = types;
			this.bundleModel = bundleModel;
		}

		public void TakeDamage(ushort damage)
		{
			hp -= (ushort)Mathf.Min(damage, MaxHp);
			onHpChange?.Invoke(Hp, MaxHp);
		}
		#endregion
	}
}
