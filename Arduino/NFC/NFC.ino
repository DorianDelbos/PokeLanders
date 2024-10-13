#include <SoftwareSerial.h>
#include <PN532_SWHSU.h>
#include <PN532.h>

SoftwareSerial SWSerial(3, 2);
PN532_SWHSU pn532swhsu(SWSerial);
PN532 nfc(pn532swhsu);
String tagId = "None";
byte nuidPICC[4];
uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };
uint8_t uidLength;

// Default key for authentication (MIFARE Classic)
uint8_t defaultKey[] = {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}; 

void setup(void) 
{
  Serial.begin(9600);
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  if (!versiondata) 
  {
    Serial.print("Didn't Find PN53x Module");
    while (1); // Halt
  }
  
  nfc.SAMConfig();
}

void loop() 
{
  readNFC();
}

void readNFC() 
{
  if (nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength)) 
  {
    for (uint8_t i = 0; i < uidLength; i++)
      nuidPICC[i] = uid[i];

    tagId = tagToString(nuidPICC);
    Serial.println("Card Detected: " + tagId);
    
    // Now read all data from the card
    readMifareCardData();
    delay(1000);
  }
}

void readMifareCardData()
{
  for (uint8_t sector = 0; sector < 16; sector++) // MIFARE Classic has 16 sectors
  {
    if (nfc.mifareclassic_AuthenticateBlock(uid, uidLength, sector * 4, 0, defaultKey)) // Authenticate sector
    {
      Serial.println("Sector " + String(sector) + ":");

      for (uint8_t block = 0; block < 4; block++) // 4 blocks per sector
      {
        uint8_t blockData[16];
        if (nfc.mifareclassic_ReadDataBlock(sector * 4 + block, blockData)) // Read each block
        {
          // Send block data to Unity (convert to string)
          String blockContent = "";
          for (uint8_t i = 0; i < 16; i++)
          {
            blockContent += String(blockData[i], HEX); // Convert to hexadecimal string
            if (i < 15) blockContent += " "; // Separate bytes with space
          }
          Serial.println("Block " + String(block) + ": " + blockContent); // Send block data to Serial
        }
        else
        {
          Serial.println("Failed to read block " + String(block));
        }
      }
    }
    else
    {
      Serial.println("Authentication failed for sector " + String(sector));
    }
  }
}

String tagToString(byte id[4]) 
{
  String tagId = "";
  for (byte i = 0; i < 4; i++)
    tagId += String(id[i]) + (i < 3 ? "." : "");
  return tagId;
}
