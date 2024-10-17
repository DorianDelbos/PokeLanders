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

    public static LanderDataNFC FromByteArray(byte[] data)
    {
        if (data.Length < 18)
        {
            Debug.LogError("Bytes not enough !");
            return null;
        }

        LanderDataNFC landerData = new LanderDataNFC();

        // Bytes 1-4
        landerData.tag = string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", data[0], data[1], data[2], data[3]);
        // Byte 5
        landerData.id = data[4];
        // Bytes 6-13
        landerData.customName = System.Text.Encoding.ASCII.GetString(data, 5, 8).Trim();
        // Byte 14
        landerData.currentHp = data[13];
        // Byte 15
        landerData.currentLevel = data[14];
        // Byte 16
        landerData.currentXp = data[15];
        // Byte 17
        landerData.height = data[16];
        // Byte 18
        landerData.weight = data[17];

        return landerData;
    }
}
