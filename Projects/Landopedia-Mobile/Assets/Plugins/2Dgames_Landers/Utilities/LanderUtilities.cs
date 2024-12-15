using System;
using System.Linq;
using Landers.API;
using UnityEngine;

namespace Landers.Utilities
{
    public static class LanderUtilities
    {
		public static LanderData RandomLander() => RandomLander((byte)UnityEngine.Random.Range(1, 100));

		public static LanderData RandomLander(byte level)
		{
			Lander[] allLanders = LanderRepository.GetAll();
			Lander landerModel = allLanders.OrderBy(x => Guid.NewGuid()).First();

			LanderData.Stats ivs = RandomStats(31);
			LanderData.Stats evs = RandomStats(252, 510);
			ushort maxHp = LanderUtilities.GetMaxHp(landerModel.stats.Where(x => x.stat == "pv").First().base_stat, level, ivs.hp, evs.hp);

			return new LanderData(
				"-1",
				landerModel.id,
				landerModel.name,
				landerModel.name,
				landerModel.description,
				LanderUtilities.GetXpByLevel(level, landerModel.base_experience),
				maxHp,
				(byte)UnityEngine.Random.Range(0, 256),
				NatureRepository.GetAll().OrderBy(x => UnityEngine.Random.value).First().name,
				new LanderData.Stats(landerModel.stats),
				ivs,
				evs,
				0, // TODO ATTACKS
				0,
				0,
				0,
				landerModel.base_experience,
				landerModel.base_height,
				landerModel.base_weight,
				UnityEngine.Random.value < 0.5f,
				UnityEngine.Random.value < (1.0f / 8192.0f),
				landerModel.types,
				$"localhost:5000/api/v1/model/{landerModel.name}.glb"
			);
		}

		public static LanderData.Stats RandomStats(int maxValue, int maxTotal = int.MaxValue - 1)
		{
			LanderData.Stats stats = new LanderData.Stats();
			System.Random random = new System.Random();
			int remainingTotal = maxTotal;
			byte[] attributes = new byte[6];

			for (int i = 0; i < attributes.Length; i++)
			{
				attributes[i] = (byte)random.Next(0, Math.Min(maxValue, remainingTotal + 1));
				remainingTotal -= attributes[i];
			}

			stats.hp = attributes[0];
			stats.attack = attributes[1];
			stats.defense = attributes[2];
			stats.specialAttack = attributes[3];
			stats.specialDefense = attributes[4];
			stats.speed = attributes[5];

			return stats;
		}

		public static ushort CalculAttackDamage(LanderData from, LanderData to, Move move)
		{
			return (ushort)((from.Level * 0.4f + 2) * from.Attack * move.power / to.Defense / 50 + 2);
		}

		public static ushort CalculSpecialAttackDamage(LanderData from, LanderData to, Move move)
		{
			return (ushort)((from.Level * 0.4f + 2) * from.SpecialAttack * move.power / to.SpecialDefense / 50 + 2);
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
		public static byte GetLevelByXp(int xp, ushort baseXp)
		{
			return (byte)(Mathf.FloorToInt(Mathf.Pow(xp / (int)baseXp, 1.0f / 3.0f)) + 1);
		}

		public static int GetXpByLevel(byte level, ushort baseXp)
		{
			return Mathf.FloorToInt((int)baseXp * Mathf.Pow((int)(level - 1), 3));
		}

		public static ushort GetMaxHp(byte baseHp, byte level, byte iv, byte ev)
		{
			return (ushort)(((2 * baseHp + iv + (ev / 4)) * level / 100) + level + 10);
		}

		public static ushort GetStatValue(byte baseState, byte level, byte iv, byte ev, float natureMultiplier)
		{
			return (ushort)((((2 * baseState + iv + (ev / 4)) * level / 100) + 5) * natureMultiplier);
		}
	}
}
