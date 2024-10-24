using System.Collections.Generic;
using System.Linq;

namespace Lander.Gameplay
{
	[System.Serializable]
    public struct LanderStats
	{
		public LanderStats(byte hp, byte attack, byte defense, byte specialAttack, byte specialDefense, byte speed)
		{
			this.hp = hp;
			this.attack = attack;
			this.defense = defense;
			this.specialAttack = specialAttack;
			this.specialDefense = specialDefense;
			this.speed = speed;
		}

		public LanderStats(List<Extern.API.Stats> stats)
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
}
