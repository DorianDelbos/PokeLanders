using dgames.Utils;

namespace Landers
{
    [System.Serializable]
    public class LanderDataNFC : System.IEquatable<LanderDataNFC>
    {
        // Block 1
        public string Tag;     // Bytes 0-3  (4 octets)
        public string Name;    // Bytes 4-17 (14 octets)
        public byte Happiness; // Byte 18    (1 octet)
        public byte Meta;      // Byte 19    (1 byte)
        public byte Nature;    // Byte 19    (1 byte)

        // Block 2
        public ushort Id;      // Bytes 20-21 (2 octets)
        public ushort Pv;      // Bytes 22-23 (2 octets)
        public int Xp;         // Bytes 24-27 (4 octets)
        public ushort Height;  // Bytes 28-29 (2 octets)
        public ushort Weight;  // Bytes 30-31 (2 octets)

        // Block 3 - Moves
        public ushort MoveId1; // Bytes 32-33 (2 octets)
        public ushort MoveId2; // Bytes 34-35 (2 octets)
        public ushort MoveId3; // Bytes 36-37 (2 octets)
        public ushort MoveId4; // Bytes 38-39 (2 octets)

        // Block 4 - IVs (Individual Values)
        public byte IvPv;      // Byte 40 (1 octet)
        public byte IvAtk;     // Byte 41 (1 octet)
        public byte IvDef;     // Byte 42 (1 octet)
        public byte IvAtkSpe;  // Byte 43 (1 octet)
        public byte IvDefSpe;  // Byte 44 (1 octet)
        public byte IvSpeed;   // Byte 45 (1 octet)

        // Block 4 - EVs (Effort Values)
        public byte EvPv;      // Byte 46 (1 octet)
        public byte EvAtk;     // Byte 47 (1 octet)
        public byte EvDef;     // Byte 48 (1 octet)
        public byte EvAtkSpe;  // Byte 49 (1 octet)
        public byte EvDefSpe;  // Byte 50 (1 octet)
        public byte EvSpeed;   // Byte 51 (1 octet)

        public LanderDataNFC() { }

        public LanderDataNFC(byte[] nfcData)
        {
            if (nfcData == null || nfcData.Length < 52) // Check if nfcData have at least 52 octets
                throw new System.ArgumentException("nfcData must be at least 48 bytes long.");

            // Block 1: Tag (Bytes 0-3)
            Tag = string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", nfcData[0], nfcData[1], nfcData[2], nfcData[3]);

            // Block 1: Name (Bytes 4-18)
            Name = System.Text.Encoding.ASCII.GetString(nfcData, 4, 14).Trim();
            Happiness = (byte)nfcData.ReadBigEndian(18, 1);   // Byte 18
            Meta = nfcData[19];                               // Byte 19
            Nature = nfcData[19].ReadBitsBigEndian(0, 5);     // Byte 19

            // Block 2: ID, HP, XP, Height, Weight
            Id = (ushort)nfcData.ReadBigEndian(20, 2);        // Bytes 20-21 (ushort)
            Pv = (ushort)nfcData.ReadBigEndian(22, 2);        // Bytes 22-23 (ushort)
            Xp = nfcData.ReadBigEndian(24, 4);                // Bytes 24-27 (int)
            Height = (ushort)nfcData.ReadBigEndian(28, 2);    // Bytes 28-29 (ushort)
            Weight = (ushort)nfcData.ReadBigEndian(30, 2);    // Bytes 30-31 (ushort)

            // Block 3: Moves IDs (Each 2 bytes as ushort)
            MoveId1 = (ushort)nfcData.ReadBigEndian(32, 2);   // Bytes 32-33 (ushort)
            MoveId2 = (ushort)nfcData.ReadBigEndian(34, 2);   // Bytes 34-35 (ushort)
            MoveId3 = (ushort)nfcData.ReadBigEndian(36, 2);   // Bytes 36-37 (ushort)
            MoveId4 = (ushort)nfcData.ReadBigEndian(38, 2);   // Bytes 38-39 (ushort)

            // Block 4: IVs (Individual Values)
            IvPv = nfcData[40];                               // Byte 40
            IvAtk = nfcData[41];                              // Byte 41
            IvDef = nfcData[42];                              // Byte 42
            IvAtkSpe = nfcData[43];                           // Byte 43
            IvDefSpe = nfcData[44];                           // Byte 44
            IvSpeed = nfcData[45];                            // Byte 45

            // Block 4: EVs (Effort Values)
            EvPv = nfcData[46];                               // Byte 46
            EvAtk = nfcData[47];                              // Byte 47
            EvDef = nfcData[48];                              // Byte 48
            EvAtkSpe = nfcData[49];                           // Byte 49
            EvDefSpe = nfcData[50];                           // Byte 50
            EvSpeed = nfcData[51];                            // Byte 51
        }

        public bool Equals(LanderDataNFC other)
        {
            if (other == null) throw new System.NullReferenceException();
            if (string.IsNullOrEmpty(Tag) || string.IsNullOrEmpty(other.Tag)) return false;
            return Tag.Equals(other.Tag);
        }
    }
}
