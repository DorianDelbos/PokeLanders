using Lander.Extern;
using Lander.Gameplay.Type;
using Lander.Maths;
using System;
using System.Linq;

namespace Lander.Gameplay
{
	public static class LanderUtils
	{
		public static LanderData RandomLander() => RandomLander((byte)UnityEngine.Random.Range(1, 100));

		public static LanderData RandomLander(byte level)
		{
			Extern.API.Lander[] allLanders = APIDataFetcher<Extern.API.Lander>.FetchArrayData("api/v1/lander");
			Extern.API.Lander landerModel = allLanders.Where(x => x.id == UnityEngine.Random.Range(1, allLanders.Length + 1)).First();
			ushort maxHp = StatsCurves.GetMaxHp(landerModel.stats.Where(x => x.stat == "pv").First().base_stat, level, 0, 0); // TODO : IV and EV

			return new LanderData(
				"-1",
				landerModel.id,
				landerModel.name,
				landerModel.name,
				landerModel.description,
				StatsCurves.GetXpByLevel(level, landerModel.base_experience),
				maxHp,
				new LanderStats(landerModel.stats),
				RandomStats(31),
				RandomStats(252, 510),
				landerModel.base_experience,
				landerModel.base_height,
				landerModel.base_weight,
				ElementaryTypeUtils.StringsToTypes(landerModel.types),
				BundleLoaderUtils.DownloadAssets(landerModel.bundle)
			);
		}

		public static LanderStats RandomStats(int maxValue, int maxTotal = int.MaxValue - 1)
		{
			LanderStats stats = new LanderStats();
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
	}
}
