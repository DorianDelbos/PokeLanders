using Unity.VisualScripting.Antlr3.Runtime;

namespace LandersLegends.Gameplay
{
    [System.Serializable]
    public class LanderDataNFC
    {
        // Block 1
        public string tag = string.Empty;      // Bytes 0-3 (4 octets)
        public string name = string.Empty;     // Bytes 4-17 (14 octets)
        public byte happiness = 0;             // Byte 18 (1 octet)
        public byte meta = 0;                  // Byte 19 (1 octet)
        public byte nature = 0;                // Also Byte 19

        // Block 2
        public ushort id = 0;                  // Bytes 20-21 (2 octets)
        public ushort hp = 0;                  // Bytes 22-23 (2 octets)
        public int xp = 0;                     // Bytes 24-27 (4 octets)
        public ushort height = 0;              // Bytes 28-29 (2 octets)
        public ushort weight = 0;              // Bytes 30-31 (2 octets)

        // Block 3
        public ushort idAttack1 = 0;           // Bytes 32-33 (2 octets)
        public ushort idAttack2 = 0;           // Bytes 34-35 (2 octets)
        public ushort idAttack3 = 0;           // Bytes 36-37 (2 octets)
        public ushort idAttack4 = 0;           // Bytes 38-39 (2 octets)

        // Block 4 - IVs (Individual Values)
        public byte ivPv = 0;                  // Byte 40 (1 octet)
        public byte ivAtk = 0;                 // Byte 41 (1 octet)
        public byte ivDef = 0;                 // Byte 42 (1 octet)
        public byte ivAtkSpe = 0;              // Byte 43 (1 octet)
        public byte ivDefSpe = 0;              // Byte 44 (1 octet)
        public byte ivSpeed = 0;               // Byte 45 (1 octet)

        // Block 4 - EVs (Effort Values)
        public byte evPv = 0;                  // Byte 46 (1 octet)
        public byte evAtk = 0;                 // Byte 47 (1 octet)
        public byte evDef = 0;                 // Byte 48 (1 octet)
        public byte evAtkSpe = 0;              // Byte 49 (1 octet)
        public byte evDefSpe = 0;              // Byte 50 (1 octet)
        public byte evSpeed = 0;               // Byte 51 (1 octet)

        // Constructor for initialization using a byte array (nfcData)
        public LanderDataNFC(byte[] nfcData)
        {
            if (nfcData == null || nfcData.Length < 52) // Vérifiez si nfcData a au moins 52 octets
            {
                throw new System.ArgumentException("nfcData must be at least 48 bytes long.");
            }

            // Block 1: Tag (Bytes 0-3)
            tag = string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", nfcData[0], nfcData[1], nfcData[2], nfcData[3]);

            // Block 1: Name (Bytes 4-18)
            name = System.Text.Encoding.ASCII.GetString(nfcData, 4, 14).Trim();
            happiness = (byte)nfcData.ReadBigEndian(18, 1);     // Byte 18
			meta = nfcData[19];                                 // Byte 19
            nature = nfcData[19].ReadBitsBigEndian(3, 5);

            // Block 2: ID, HP, XP, Height, Weight
            id = (ushort)nfcData.ReadBigEndian(20, 2);          // Bytes 20-21 (ushort)
            hp = (ushort)nfcData.ReadBigEndian(22, 2);          // Bytes 22-23 (ushort)
			xp = nfcData.ReadBigEndian(24, 4);                  // Bytes 24-27 (int)
			height = (ushort)nfcData.ReadBigEndian(28, 2);      // Bytes 28-29 (ushort)
            weight = (ushort)nfcData.ReadBigEndian(30, 2);      // Bytes 30-31 (ushort)

			// Block 3: Attack IDs (Each 2 bytes as ushort)
			idAttack1 = (ushort)nfcData.ReadBigEndian(32, 2);   // Bytes 32-33 (ushort)
            idAttack2 = (ushort)nfcData.ReadBigEndian(34, 2);   // Bytes 34-35 (ushort)
            idAttack3 = (ushort)nfcData.ReadBigEndian(36, 2);   // Bytes 36-37 (ushort)
            idAttack4 = (ushort)nfcData.ReadBigEndian(38, 2);   // Bytes 38-39 (ushort)

			// Block 4: IVs (Individual Values)
			ivPv = nfcData[40];                                 // Byte 40
            ivAtk = nfcData[41];                                // Byte 41
            ivDef = nfcData[42];                                // Byte 42
            ivAtkSpe = nfcData[43];                             // Byte 43
            ivDefSpe = nfcData[44];                             // Byte 44
            ivSpeed = nfcData[45];                              // Byte 45

            // Block 4: EVs (Effort Values)
            evPv = nfcData[46];                                 // Byte 46
            evAtk = nfcData[47];                                // Byte 47
            evDef = nfcData[48];                                // Byte 48
            evAtkSpe = nfcData[49];                             // Byte 49
            evDefSpe = nfcData[50];                             // Byte 50
            evSpeed = nfcData[51];                              // Byte 51
        }
	}
}
