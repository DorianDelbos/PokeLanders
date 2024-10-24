namespace Lander.Gameplay
{
    [System.Serializable]
    public class LanderDataNFC
    {
        public string tag = string.Empty;
        public int id = 0;
        public string name = string.Empty;
        public int hp = 0;
        public int level = 0;
        public int xp = 0;
        public int height = 0;
        public int weight = 0;

        public LanderDataNFC(string tag, int id, string name, int hp, int level, int xp, int height, int weight)
        {
            this.tag = tag;
            this.id = id;
            this.name = name;
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
            name = System.Text.Encoding.ASCII.GetString(nfcData, 5, 8).Trim();
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
}
