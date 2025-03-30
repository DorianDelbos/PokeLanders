using dgames.Utils;
using System.Linq;

namespace Landers
{
    [System.Serializable]
    public class LanderData
    {
        public string Tag;
        public ushort Id;
        public string Species;
        public string Name;
        public string Description;

        public int Xp;
        public ushort Pv;
        public byte Happiness;
        public byte Nature;
        public byte[] Stats;
        public byte[] Ivs;
        public byte[] Evs;

        public ushort[] Moves;
        public ushort BaseXp;
        public ushort Height;
        public ushort Weight;
        public bool IsMale;
        public bool IsShiny;
        public string[] Types;
        public string ModelUrl;

        public byte Level => Utils.LanderUtils.GetLevelByXp(Xp, BaseXp);
        public ushort MaxHp => Utils.LanderUtils.GetMaxHp(Stats[(int)StatsEnum.Pv], Level, Ivs[(int)StatsEnum.Pv], Evs[(int)StatsEnum.Pv]);

        public LanderData() { }

        public LanderData(LanderDataNFC nfcData, API.Lander landerModel)
        {
            Tag = nfcData.Tag;
            Id = nfcData.Id;
            Name = landerModel.name;
            Species = nfcData.Name;
            Description = landerModel.description;
            Xp = nfcData.Xp;
            Pv = nfcData.Pv;
            Happiness = nfcData.Happiness;
            Nature = nfcData.Nature;
            Stats = landerModel.stats.Select(x => x.base_stat).ToArray();
            Ivs = new byte[] { nfcData.IvPv, nfcData.IvAtk, nfcData.IvDef, nfcData.IvAtkSpe, nfcData.IvDefSpe, nfcData.IvSpeed };
            Evs = new byte[] { nfcData.EvPv, nfcData.EvAtk, nfcData.EvDef, nfcData.EvAtkSpe, nfcData.EvDefSpe, nfcData.EvSpeed };
            Moves = new ushort[] { nfcData.MoveId1, nfcData.MoveId2, nfcData.MoveId3, nfcData.MoveId4 };
            BaseXp = landerModel.base_experience;
            Height = nfcData.Height;
            Weight = nfcData.Weight;
            IsMale = nfcData.Meta.GetBit(0);
            IsShiny = nfcData.Meta.GetBit(1);
            Types = landerModel.types.ToArray();
            ModelUrl = landerModel.model;
        }
    }
}
