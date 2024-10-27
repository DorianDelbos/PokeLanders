using UnityEngine;

namespace Lander.Maths
{
    public static class StatsCurves
    {
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

        public static ushort GetStatValue(byte level, byte iv, byte ev, byte baseState = 0)
        {
            return (ushort)(((2 * baseState + iv + (ev / 4)) * level / 100) + 5);
        }
    }
}