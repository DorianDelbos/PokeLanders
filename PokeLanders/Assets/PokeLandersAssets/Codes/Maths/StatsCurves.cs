using UnityEngine;

public static class StatsCurves
{
    private static float GetConstantLevels(int levelMax, int xpMax)
    {
        return levelMax / Mathf.Sqrt(xpMax - 1);
    }

    public static int GetLevelByXp(int xp, int levelMax, int xpMax)
    {
        return Mathf.FloorToInt(GetConstantLevels(levelMax, xpMax) * Mathf.Sqrt(xp) + 1);
    }

    public static int GetXpByLevel(int level, int levelMax, int xpMax)
    {
        return Mathf.FloorToInt((xpMax - 1) / GetConstantLevels(level, xpMax));
    }

    public static int GetMaxHpByLevel(int level)
    {
        return (20 + 2 * (level - 1));
    }
}
