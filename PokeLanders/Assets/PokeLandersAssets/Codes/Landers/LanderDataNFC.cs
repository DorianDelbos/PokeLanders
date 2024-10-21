[System.Serializable]
public class LanderDataNFC
{
    public string tag = string.Empty;
    public ushort id = 0;
	public string customName = string.Empty;
    public ushort hp = 0;
    public ushort level = 0;
    public ushort xp = 0;
    public ushort height = 0;
    public ushort weight = 0;

    public LanderDataNFC()
    {
        tag = string.Empty;
        id = 0;
		customName = string.Empty;
		hp = 0;
        level = 0;
        xp = 0;
        height = 0;
        weight = 0;
    }

	public LanderDataNFC(string tag, ushort id, string customName, ushort hp, ushort level, ushort xp, ushort height, ushort weight)
	{
		this.tag = tag;
		this.id = id;
		this.customName = customName;
		this.hp = hp;
		this.level = level;
		this.xp = xp;
		this.height = height;
		this.weight = weight;
	}

	public LanderDataNFC(byte[] nfcData)
    {
        // Bytes 1-4
        tag = string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", nfcData[0], nfcData[1], nfcData[2], nfcData[3]);
        // Byte 5
        id = nfcData[4];
        // Bytes 6-13
        customName = System.Text.Encoding.ASCII.GetString(nfcData, 5, 8).Trim();
        // Byte 14
        hp = nfcData[13];
        // Byte 15
        level = nfcData[14];
        // Byte 16
        xp = nfcData[15];
        // Byte 17
        height = nfcData[16];
        // Byte 18
        weight = nfcData[17];
    }
}
