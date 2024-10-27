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
			Extern.API.Lander landerModel = allLanders.OrderBy(x => Guid.NewGuid()).First();

			LanderData.Stats ivs = RandomStats(31);
			LanderData.Stats evs = RandomStats(252, 510);
            ushort maxHp = StatsCurves.GetMaxHp(landerModel.stats.Where(x => x.stat == "pv").First().base_stat, level, ivs.hp, evs.hp);

			return new LanderData(
				"-1",
				landerModel.id,
				landerModel.name,
				landerModel.name,
				landerModel.description,
				StatsCurves.GetXpByLevel(level, landerModel.base_experience),
				maxHp,
				new LanderData.Stats(landerModel.stats),
                ivs,
                evs,
				landerModel.base_experience,
				landerModel.base_height,
				landerModel.base_weight,
				ElementaryTypeUtils.StringsToTypes(landerModel.types),
				BundleLoaderUtils.DownloadAssets(landerModel.bundle)
			);
		}

		public static LanderData.Stats RandomStats(int maxValue, int maxTotal = int.MaxValue - 1)
		{
			LanderData.Stats stats = new LanderData.Stats();
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
