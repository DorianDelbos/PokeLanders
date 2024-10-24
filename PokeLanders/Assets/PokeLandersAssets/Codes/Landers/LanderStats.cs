using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Lander.Gameplay
{
	public struct LanderStats
	{
		public LanderStats(int baseHp, int attack, int defense, int specialAttack, int specialDefense, int speed)
		{
			this.baseHp = baseHp;
			this.attack = attack;
			this.defense = defense;
			this.specialAttack = specialAttack;
			this.specialDefense = specialDefense;
			this.speed = speed;
		}

		public LanderStats(List<Extern.API.Stats> stats)
		{
			baseHp = stats.Where(x => x.stat == "pv").First().base_stat;
			attack = stats.Where(x => x.stat == "attack").First().base_stat;
			defense = stats.Where(x => x.stat == "defense").First().base_stat;
			specialAttack = stats.Where(x => x.stat == "special-attack").First().base_stat;
			specialDefense = stats.Where(x => x.stat == "special-defense").First().base_stat;
			speed = stats.Where(x => x.stat == "speed").First().base_stat;
		}

		public int baseHp;
		public int attack;
		public int defense;
		public int specialAttack;
		public int specialDefense;
		public int speed;
	}
}
