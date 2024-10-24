using System;

namespace Lander.Gameplay
{
    [Serializable]
    public class LanderDataNFC
    {
        // Block 1
        public string tag = string.Empty;      // Bytes 0-3 (4 octets)
        public string name = string.Empty;     // Bytes 4-15 (12 octets)

        // Block 2
        public ushort id = 0;                  // Bytes 16-17 (2 octets)
        public ushort hp = 0;                  // Bytes 18-19 (2 octets)
        public int xp = 0;                     // Bytes 20-23 (4 octets)
        public ushort height = 0;              // Bytes 24-25 (2 octets)
        public ushort weight = 0;              // Bytes 26-27 (2 octets)

        // Block 3
        public ushort idAttack1 = 0;           // Bytes 28-29 (2 octets)
        public ushort idAttack2 = 0;           // Bytes 30-31 (2 octets)
        public ushort idAttack3 = 0;           // Bytes 32-33 (2 octets)
        public ushort idAttack4 = 0;           // Bytes 34-35 (2 octets)

        // Block 4 - IVs (Individual Values)
        public byte ivPv = 0;                  // Byte 36 (1 octet)
        public byte ivAtk = 0;                 // Byte 37 (1 octet)
        public byte ivDef = 0;                 // Byte 38 (1 octet)
        public byte ivAtkSpe = 0;              // Byte 39 (1 octet)
        public byte ivDefSpe = 0;              // Byte 40 (1 octet)
        public byte ivSpeed = 0;               // Byte 41 (1 octet)

        // Block 4 - EVs (Effort Values)
        public byte evPv = 0;                  // Byte 42 (1 octet)
        public byte evAtk = 0;                 // Byte 43 (1 octet)
        public byte evDef = 0;                 // Byte 44 (1 octet)
        public byte evAtkSpe = 0;              // Byte 45 (1 octet)
        public byte evDefSpe = 0;              // Byte 46 (1 octet)
        public byte evSpeed = 0;               // Byte 47 (1 octet)

        // Constructor for initialization using a byte array (nfcData)
        public LanderDataNFC(byte[] nfcData)
        {
            if (nfcData == null || nfcData.Length < 48) // Vérifiez si nfcData a au moins 48 octets
            {
                throw new ArgumentException("nfcData must be at least 48 bytes long.");
            }

            // Block 1: Tag (Bytes 0-3)
            tag = string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", nfcData[0], nfcData[1], nfcData[2], nfcData[3]);

            // Block 1: Name (Bytes 4-15)
            name = System.Text.Encoding.ASCII.GetString(nfcData, 4, 12).Trim();

            // Block 2: ID, HP, XP, Height, Weight
            id = ReadUInt16BigEndian(nfcData, 16);       // Bytes 16-17 (ushort)
            hp = ReadUInt16BigEndian(nfcData, 18);       // Bytes 18-19 (ushort)
            xp = ReadInt32BigEndian(nfcData, 20);        // Bytes 20-23 (int)
            height = ReadUInt16BigEndian(nfcData, 24);   // Bytes 24-25 (ushort)
            weight = ReadUInt16BigEndian(nfcData, 26);   // Bytes 26-27 (ushort)

            // Block 3: Attack IDs (Each 2 bytes as ushort)
            idAttack1 = ReadUInt16BigEndian(nfcData, 28); // Bytes 28-29 (ushort)
            idAttack2 = ReadUInt16BigEndian(nfcData, 30); // Bytes 30-31 (ushort)
            idAttack3 = ReadUInt16BigEndian(nfcData, 32); // Bytes 32-33 (ushort)
            idAttack4 = ReadUInt16BigEndian(nfcData, 34); // Bytes 34-35 (ushort)

            // Block 4: IVs (Individual Values)
            ivPv = nfcData[36];                            // Byte 36
            ivAtk = nfcData[37];                           // Byte 37
            ivDef = nfcData[38];                           // Byte 38
            ivAtkSpe = nfcData[39];                        // Byte 39
            ivDefSpe = nfcData[40];                        // Byte 40
            ivSpeed = nfcData[41];                         // Byte 41

            // Block 4: EVs (Effort Values)
            evPv = nfcData[42];                            // Byte 42
            evAtk = nfcData[43];                           // Byte 43
            evDef = nfcData[44];                           // Byte 44
            evAtkSpe = nfcData[45];                        // Byte 45
            evDefSpe = nfcData[46];                        // Byte 46
            evSpeed = nfcData[47];                         // Byte 47
        }

        private ushort ReadUInt16BigEndian(byte[] data, int startIndex)
        {
            return (ushort)((data[startIndex] << 8) | data[startIndex + 1]);
        }

        private int ReadInt32BigEndian(byte[] data, int startIndex)
        {
            return (data[startIndex] << 24) |
                   (data[startIndex + 1] << 16) |
                   (data[startIndex + 2] << 8) |
                   (data[startIndex + 3]);
        }
    }
}