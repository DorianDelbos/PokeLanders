using UnityEngine;

public static class StatsCurves
{
    private static int levelMax = 99;
    private static int xpMax = 100000;

	private static float GetConstantLevels()
    {
        return levelMax / Mathf.Sqrt(xpMax - 1);
    }

    public static int GetLevelByXp(int xp)
    {
        return Mathf.FloorToInt(GetConstantLevels() * Mathf.Sqrt(xp) + 1);
    }

    public static int GetXpByLevel(int level)
    {
        int v = Mathf.FloorToInt((level - 1) / GetConstantLevels());
		return v * v;
    }

    public static int GetMaxHpByLevel(int level)
    {
        return (20 + 2 * (level - 1));
    }
}
