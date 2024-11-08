using System.Text;
using System;
using Unity.VisualScripting.Antlr3.Runtime;
using LandersLegends.Extern.API;

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
            nature = nfcData[19].ReadBitsBigEndian(0, 5);

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

        public LanderDataNFC(Lander.MainData mainData, Lander.StatsData statsData, Lander.AttacksData attacksData, Lander.OtherData otherData)
        {
            tag = mainData.tag;
            name = mainData.name;
            happiness = statsData.happiness;
            meta = (byte)(((otherData.isMale ? 1 : 0) & 0x01) | ((otherData.isShiny ? 1 : 0) & 0x02));
            nature = (byte)NatureRepository.GetIdByName(statsData.nature);
            id = mainData.id;
            hp = statsData.hp;
            xp = statsData.xp;
            height = otherData.height;
            weight = otherData.weight;
            idAttack1 = attacksData.attack1;
            idAttack2 = attacksData.attack2;
            idAttack3 = attacksData.attack3;
            idAttack4 = attacksData.attack4;
            ivPv = statsData.ivs.hp;
            ivAtk = statsData.ivs.attack;
            ivDef = statsData.ivs.defense;
            ivAtkSpe = statsData.ivs.specialAttack;
            ivDefSpe = statsData.ivs.specialDefense;
            ivSpeed = statsData.ivs.speed;
            evPv = statsData.evs.hp;
            evAtk = statsData.evs.attack;
            evDef = statsData.evs.defense;
            evAtkSpe = statsData.evs.specialAttack;
            evDefSpe = statsData.evs.specialDefense;
            evSpeed = statsData.evs.speed;
        }

        public byte[] ToBytes()
        {
            byte[] data = new byte[52];

            // Block 1: Tag (Bytes 0-3)
            string[] tagBytes = tag.Split(' ');
            for (int i = 0; i < 4; i++)
            {
                data[i] = Convert.ToByte(tagBytes[i], 16);
            }

            // Block 1: Name (Bytes 4-17)
            byte[] nameBytes = Encoding.ASCII.GetBytes(name.PadRight(14));
            Array.Copy(nameBytes, 0, data, 4, 14);

            // Block 1: Happiness, Meta, Nature (Bytes 18-19)
            data[18] = happiness;
            data[19] = meta;
            data[19] = data[19].ReverseBits8();
            data[19] |= nature;
            data[19] = data[19].ReverseBits8();

            // Block 2: ID, HP, XP, Height, Weight (Bytes 20-31)
            // 1. Copy id (2 bytes) to position 20
            Array.Copy(BitConverter.GetBytes(id), 0, data, 20, 2);
            Array.Reverse(data, 20, 2); // Reverse bytes

            // 2. Copy hp (2 bytes) to position 22
            Array.Copy(BitConverter.GetBytes(hp), 0, data, 22, 2);
            Array.Reverse(data, 22, 2); // Reverse bytes

            // 3. Copy xp (4 bytes) to position 24
            Array.Copy(BitConverter.GetBytes(xp), 0, data, 24, 4);
            Array.Reverse(data, 24, 4); // Reverse bytes

            // 4. Copy height (2 bytes) to position 28
            Array.Copy(BitConverter.GetBytes(height), 0, data, 28, 2);
            Array.Reverse(data, 28, 2); // Reverse bytes

            // 5. Copy weight (2 bytes) to position 30
            Array.Copy(BitConverter.GetBytes(weight), 0, data, 30, 2);
            Array.Reverse(data, 30, 2); // Reverse bytes

            // Block 3: Copying Attack IDs (all 2 bytes)
            Array.Copy(BitConverter.GetBytes(idAttack1), 0, data, 32, 2);
            Array.Reverse(data, 32, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(idAttack2), 0, data, 34, 2);
            Array.Reverse(data, 34, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(idAttack3), 0, data, 36, 2);
            Array.Reverse(data, 36, 2); // Reverse bytes

            Array.Copy(BitConverter.GetBytes(idAttack4), 0, data, 38, 2);
            Array.Reverse(data, 38, 2); // Reverse bytes

            // Block 4: IVs (Bytes 40-45)
            data[40] = ivPv;
            data[41] = ivAtk;
            data[42] = ivDef;
            data[43] = ivAtkSpe;
            data[44] = ivDefSpe;
            data[45] = ivSpeed;

            // Block 4: EVs (Bytes 46-51)
            data[46] = evPv;
            data[47] = evAtk;
            data[48] = evDef;
            data[49] = evAtkSpe;
            data[50] = evDefSpe;
            data[51] = evSpeed;

            return data;
        }
    }
}
