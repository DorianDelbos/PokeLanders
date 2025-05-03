using dgames.Utils;
using Landers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Landers.Utils
{
    public static class LanderUtils
    {
        public static LanderData RandomLander() => RandomLander((byte)UnityEngine.Random.Range(1, 100));

        public static LanderData RandomLander(byte level, int levelOffset = 0)
        {
            level += (byte)Mathf.Clamp(UnityEngine.Random.Range(-levelOffset, levelOffset + 1), 1, 100);

            Lander[] allLanders = LanderRepository.Instance.GetAll();
            Lander landerModel = allLanders.OrderBy(x => Guid.NewGuid()).First();

            byte[] baseStats = new byte[] { landerModel.stats[(int)StatsEnum.Pv].base_stat, landerModel.stats[(int)StatsEnum.Attack].base_stat, landerModel.stats[(int)StatsEnum.Defense].base_stat, landerModel.stats[(int)StatsEnum.AttackSpecial].base_stat, landerModel.stats[(int)StatsEnum.DefenseSpecial].base_stat, landerModel.stats[(int)StatsEnum.Speed].base_stat };
            byte[] ivs = RandomStats(31);
            byte[] evs = RandomStats(252, 510);
            ushort maxHp = GetMaxHp(landerModel.stats.First(x => x.stat == "pv").base_stat, level, ivs[(int)StatsEnum.Pv], evs[(int)StatsEnum.Pv]);

            return new LanderData()
            {
                Tag = GenerateRandomHexString(8),
                Id = landerModel.id,
                Species = landerModel.name,
                Name = landerModel.name,
                Description = landerModel.description,
                Xp = GetXpByLevel(level, landerModel.base_experience),
                Pv = maxHp,
                Happiness = (byte)UnityEngine.Random.Range(0, 256),
                Nature = (byte)UnityEngine.Random.Range(1, 26),
                Stats = landerModel.stats.Select(x => x.base_stat).ToArray(),
                Ivs = ivs,
                Evs = evs,
                Moves = GetRandomsMoves(landerModel.id, level),
                BaseXp = landerModel.base_experience,
                Height = landerModel.base_height,
                Weight = landerModel.base_weight,
                IsMale = UnityEngine.Random.value < 0.5f,
                IsShiny = UnityEngine.Random.value < (1.0f / 8192.0f),
                Types = landerModel.types.ToArray(),
                ModelUrl = $"{LanderConst.Url}api/v1/model/{landerModel.name}.glb"
            };
        }

        public static LanderDataNFC ToLanderDataNfc(this LanderData data)
        {
            return new LanderDataNFC()
            {
                Tag = data.Tag,
                Name = data.Name,
                Happiness = data.Happiness,
                Meta = (byte)(((data.IsMale ? 1 : 0) & 0x01) | ((data.IsShiny ? 1 : 0) & 0x02)),
                Nature = data.Nature,
                Id = data.Id,
                Pv = data.Pv,
                Xp = data.Xp,
                Height = data.Height,
                Weight = data.Weight,
                MoveId1 = data.Moves[0],
                MoveId2 = data.Moves[1],
                MoveId3 = data.Moves[2],
                MoveId4 = data.Moves[3],
                IvPv = data.Ivs[(int)StatsEnum.Pv],
                IvAtk = data.Ivs[(int)StatsEnum.Attack],
                IvDef = data.Ivs[(int)StatsEnum.Defense],
                IvAtkSpe = data.Ivs[(int)StatsEnum.AttackSpecial],
                IvDefSpe = data.Ivs[(int)StatsEnum.DefenseSpecial],
                IvSpeed = data.Ivs[(int)StatsEnum.Speed],
                EvPv = data.Evs[(int)StatsEnum.Pv],
                EvAtk = data.Evs[(int)StatsEnum.Attack],
                EvDef = data.Evs[(int)StatsEnum.Defense],
                EvAtkSpe = data.Evs[(int)StatsEnum.AttackSpecial],
                EvDefSpe = data.Evs[(int)StatsEnum.DefenseSpecial],
                EvSpeed = data.Evs[(int)StatsEnum.Speed]
            };
        }

        private static string GenerateRandomHexString(int length)
        {
            System.Random random = new System.Random();
            char[] hexChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                // Generate a random integer between 0 and 15 (inclusive), then convert to hex
                hexChars[i] = "0123456789ABCDEF"[random.Next(16)];
            }
            return new string(hexChars);
        }

        public static ushort[] GetRandomsMoves(int landerId, byte level)
        {
            ushort[] result = new ushort[] { 0, 0, 0, 0 };
            Queue<Moves> movesAvailable = LanderRepository.Instance
                .GetById(landerId).moves
                .Where(x => x.move_learned_details.level_learned_at < level)
                .OrderBy(x => UnityEngine.Random.value)
                .ToQueue();

            for (int i = 0; i < 4; i++)
            {
                if (movesAvailable.IsEmpty())
                    break;

                Moves randomMove = movesAvailable.Dequeue();
                result[i] = (ushort)MoveRepository.Instance.GetByName(randomMove.move).id;
            }

            return result;
        }

        public static byte[] RandomStats(int maxValue, int maxTotal = int.MaxValue - 1)
        {
            System.Random random = new System.Random();
            int remainingTotal = maxTotal;
            byte[] attributes = new byte[Enum.GetNames(typeof(StatsEnum)).Length];

            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i] = (byte)random.Next(0, Math.Min(maxValue, remainingTotal + 1));
                remainingTotal -= attributes[i];
            }

            return attributes;
        }

        public static ushort CalculAttackDamage(LanderData from, LanderData to, Move move)
        {
            return (ushort)((from.Level * 0.4f + 2) * from.Stats[(int)StatsEnum.Attack] * move.power / to.Stats[(int)StatsEnum.Defense] / 50 + 2);
        }

        public static ushort CalculSpecialAttackDamage(LanderData from, LanderData to, Move move)
        {
            return (ushort)((from.Level * 0.4f + 2) * from.Stats[(int)StatsEnum.AttackSpecial] * move.power / to.Stats[(int)StatsEnum.DefenseSpecial] / 50 + 2);
        }

        public static byte GetLevelByXp(int xp, ushort baseXp)
        {
            return (byte)(Mathf.FloorToInt(Mathf.Pow((float)xp / baseXp, 1.0f / 3.0f)) + 1);
        }

        public static int GetXpByLevel(byte level, ushort baseXp)
        {
            return Mathf.FloorToInt(baseXp * Mathf.Pow(level - 1, 3));
        }

        public static ushort GetMaxHp(byte baseHp, byte level, byte iv, byte ev)
        {
            return (ushort)(((2 * baseHp + iv + (ev / 4)) * level / 100) + level + 10);
        }

        public static float[] GetNatureMultiplier(byte nature)
        {
            return GetNatureMultiplier(NatureRepository.Instance.GetById(nature));
        }

        public static float[] GetNatureMultiplier(Nature nature)
        {
            Stat[] statsCached = StatRepository.Instance.GetAll();
            int statsLenght = statsCached.Count();
            float[] multipliers = new float[statsLenght];

            for (int i = 0; i < statsLenght; i++)
            {
                Stat stat = statsCached.First(x => x.id == i + 1);
                multipliers[i] = 1.0f;

                if (nature.stat_changes[0].stat == nature.stat_changes[1].stat)
                    continue;

                foreach (var statChange in nature.stat_changes)
                {
                    // Vérifier si la nature affecte cette statistique
                    if (stat.name == statChange.stat)
                    {
                        if (statChange.change == 1)
                        {
                            multipliers[i] = 1.1f;
                        }
                        else if (statChange.change == -1)
                        {
                            multipliers[i] = 0.9f;
                        }
                    }
                }
            }

            return multipliers;
        }

        public static ushort GetStatValue(StatsEnum stat, LanderData data)
        {
            return GetStatValue(data.Stats[(int)stat], data.Level, data.Ivs[(int)stat], data.Evs[(int)stat], GetNatureMultiplier(data.Nature)[(int)stat]);
        }

        public static ushort GetStatValue(byte baseState, byte level, byte iv, byte ev, float natureMultiplier)
        {
            return (ushort)((((2 * baseState + iv + (ev / 4)) * level / 100) + 5) * natureMultiplier);
        }

        public static string GetHeightInInches(int height)
        {
            int kilograms = height / 100;
            int grams = height % 100;

            return $"{kilograms}'{grams:D2}''";
        }

        public static string GetWeightInPounds(int weight)
        {
            return $"{(weight / 100m).ToString("F2")} lbs";
        }

        public static string GetHeightInMeters(int height)
        {
            decimal heightInMeters = height * 0.0254m;
            return heightInMeters.ToString("F2") + " m";
        }

        public static string GetWeightInKilograms(int weight)
        {
            decimal weightInKilograms = weight * 0.453592m;
            return weightInKilograms.ToString("F2") + " kg";
        }

        public static byte[] ToBytes(this LanderDataNFC nfc)
        {
            byte[] data = new byte[52];

            // Block 1: Tag (Bytes 0-3)
            string[] tagBytes = nfc.Tag.Split(' ');
            for (int i = 0; i < 4; i++)
            {
                data[i] = Convert.ToByte(tagBytes[i], 16);
            }

            // Block 1: Name (Bytes 4-17)
            byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(nfc.Name.PadRight(14));
            Array.Copy(nameBytes, 0, data, 4, 14);

            // Block 1: Happiness, Meta, Nature (Bytes 18-19)
            data[18] = nfc.Happiness;
            data[19] = nfc.Meta;
            data[19] = data[19].ReverseBits8();
            data[19] |= nfc.Nature;
            data[19] = data[19].ReverseBits8();

            // Block 2: ID, HP, XP, Height, Weight (Bytes 20-31)
            // 1. Copy id (2 bytes) to position 20
            Array.Copy(BitConverter.GetBytes(nfc.Id), 0, data, 20, 2);
            Array.Reverse(data, 20, 2); // Reverse bytes

            // 2. Copy hp (2 bytes) to position 22
            Array.Copy(BitConverter.GetBytes(nfc.Pv), 0, data, 22, 2);
            Array.Reverse(data, 22, 2); // Reverse bytes

            // 3. Copy xp (4 bytes) to position 24
            Array.Copy(BitConverter.GetBytes(nfc.Xp), 0, data, 24, 4);
            Array.Reverse(data, 24, 4); // Reverse bytes

            // 4. Copy height (2 bytes) to position 28
            Array.Copy(BitConverter.GetBytes(nfc.Height), 0, data, 28, 2);
            Array.Reverse(data, 28, 2); // Reverse bytes

            // 5. Copy weight (2 bytes) to position 30
            Array.Copy(BitConverter.GetBytes(nfc.Weight), 0, data, 30, 2);
            Array.Reverse(data, 30, 2); // Reverse bytes

            // Block 3: Copying Attack IDs (all 2 bytes)
            Array.Copy(BitConverter.GetBytes(nfc.MoveId1), 0, data, 32, 2);
            Array.Reverse(data, 32, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(nfc.MoveId2), 0, data, 34, 2);
            Array.Reverse(data, 34, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(nfc.MoveId3), 0, data, 36, 2);
            Array.Reverse(data, 36, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(nfc.MoveId4), 0, data, 38, 2);
            Array.Reverse(data, 38, 2); // Reverse bytes

            // Block 4: IVs (Bytes 40-45)
            data[40] = nfc.IvPv;
            data[41] = nfc.IvAtk;
            data[42] = nfc.IvDef;
            data[43] = nfc.IvAtkSpe;
            data[44] = nfc.IvDefSpe;
            data[45] = nfc.IvSpeed;

            // Block 4: EVs (Bytes 46-51)
            data[46] = nfc.EvPv;
            data[47] = nfc.EvAtk;
            data[48] = nfc.EvDef;
            data[49] = nfc.EvAtkSpe;
            data[50] = nfc.EvDefSpe;
            data[51] = nfc.EvSpeed;

            return data;
        }
    }
}
