using LandersLegends.Extern;
using LandersLegends.Extern.API;
using LandersLegends.Maths;
using System;
using System.Linq;

namespace LandersLegends.Gameplay
{
	public static class LanderUtils
	{
		public static Lander RandomLander() => RandomLander((byte)UnityEngine.Random.Range(1, 100));

		public static Lander RandomLander(byte level)
		{
			Extern.API.Lander[] allLanders = DataFetcher<Extern.API.Lander>.FetchArrayData("api/v1/lander");
			Extern.API.Lander landerModel = allLanders.OrderBy(x => Guid.NewGuid()).First();

			Lander.Stats ivs = RandomStats(31);
			Lander.Stats evs = RandomStats(252, 510);
            ushort maxHp = StatsCurves.GetMaxHp(landerModel.stats.Where(x => x.stat == "pv").First().base_stat, level, ivs.hp, evs.hp);

			return new Lander(
				"-1",
				landerModel.id,
				landerModel.name,
				landerModel.name,
				landerModel.description,
				StatsCurves.GetXpByLevel(level, landerModel.base_experience),
				maxHp,
				(byte)UnityEngine.Random.Range(0, 256),
				NatureRepository.GetAll().OrderBy(x => UnityEngine.Random.value).First().name,
				new Lander.Stats(landerModel.stats),
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

		public static Lander.Stats RandomStats(int maxValue, int maxTotal = int.MaxValue - 1)
		{
			Lander.Stats stats = new Lander.Stats();
			Random random = new Random();
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

		public static ushort CalculAttackDamage(Lander from, Lander to, Move move)
		{
			return (ushort)((from.Level * 0.4f + 2) * from.Attack * move.power / to.Defense / 50 + 2);
		}

		public static ushort CalculSpecialAttackDamage(Lander from, Lander to, Move move)
		{
			return (ushort)((from.Level * 0.4f + 2) * from.SpecialAttack * move.power / to.SpecialDefense / 50 + 2);
		}
	}
}
