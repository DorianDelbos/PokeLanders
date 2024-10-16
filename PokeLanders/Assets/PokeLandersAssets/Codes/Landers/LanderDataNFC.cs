using UnityEngine;

[System.Serializable]
public class LanderDataNFC
{
    public string tag = string.Empty;
    public ushort id = 0;
	public string customName = string.Empty;
    public ushort currentHp = 0;
    public ushort currentLevel = 0;
    public ushort currentXp = 0;
    public ushort height = 0;
    public ushort weight = 0;
}
