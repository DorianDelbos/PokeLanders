//#define DebugMode
//#define WritingMode

#include <SoftwareSerial.h>
#include <PN532_SWHSU.h>
#include <PN532.h>

// Pins variables
SoftwareSerial SWSerial(3, 2);
// Pn532 utils
PN532_SWHSU pn532swhsu(SWSerial);
PN532 nfc(pn532swhsu);

uint8_t uid[8] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
uint8_t uidLength;
uint8_t blocksData[16];

void setup(void) 
{
  Serial.begin(9600);
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  
  if (!versiondata) 
  {
#ifdef DebugMode
    Serial.println("Didn't Find PN53x Module");
#endif
    while (1); // Halt
  }
#ifdef DebugMode
  else
  {
    Serial.println("Pn532 ready, Start NFC Read/Write !");
    Serial.println("Waiting a card ...");
  }
#endif
  
  nfc.SAMConfig();
}

void loop() 
{
  if (tagIsDetect())
  {    
    delay(1000);

#ifdef WritingMode
    // ID, Custom name (x8), Current HP, Current Level, Current XP, Height, Weight
    //uint8_t data[16] = { 1, 'K', 'a', 'r', 'o', 't', ' ', ' ', ' ', 47, 6, 241, 102, 14, 0x00, 0x00};
    uint8_t data[16] = { 2, 'M', 'a', 'r', 'i', 'o', 'B', 'r', 'o', 12, 65, 125, 23, 255, 0x00, 0x00};
    if (WriteBlock(1, data))
    {
      Serial.println("Data correctly write");
    }
    
    delay(1000);
#endif

    for (int j = 0; j < 16; j++) {
        blocksData[j] = 0x00;
    }
    ReadBlock(1);

    SendData();

    delay(1000);
    
#ifdef DebugMode
    Serial.println("Waiting a card ...");
#endif
  }
#ifndef DebugMode
  else
  {
    Serial.println("-1");
  }
#endif
}

bool tagIsDetect() 
{
  bool readSuccess = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength);

#ifdef DebugMode
  if (readSuccess) 
  {
    Serial.print("tag:");
    nfc.PrintHex(&uid[0], uidLength);
  }
#endif

  return readSuccess;
}

bool ReadBlock(uint8_t blockNumber)
{
    uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
    if (!nfc.mifareclassic_AuthenticateBlock(uid, uidLength, blockNumber, 1, keya))
    {
#ifdef DebugMode
      Serial.println("Failed to authenticate block !");
#endif
      return false;
    }

    uint8_t data[16];
    if (!nfc.mifareclassic_ReadDataBlock(blockNumber, data))
    {
#ifdef DebugMode
      Serial.println("Failed to read data block !");
#endif
      return false;
    }

    for (int j = 0; j < 16; j++) {
      blocksData[j] = data[j];
    }

    // Serial.println(uint8ArrayToString(data, 16));

    return true;
}

#ifdef WritingMode
bool WriteBlock(uint8_t blockNumber, uint8_t *data)
{
    uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
    if (!nfc.mifareclassic_AuthenticateBlock(uid, uidLength, blockNumber, 1, keya))
    {
#ifdef DebugMode
      Serial.println("Failed to authenticate block !");
#endif
      return false;
    }
    
    if (!nfc.mifareclassic_WriteDataBlock(blockNumber, data))
    {
#ifdef DebugMode
      Serial.println("Failed to write data block !");
#endif
      return false;
    }

    return true;
}
#endif

void SendData()
{
  uint8_t name[8] = { blocksData[1], blocksData[2], blocksData[3], blocksData[4], blocksData[5], blocksData[6], blocksData[7], blocksData[8] };
  String result = "{\"tag\": \"" + TagToString(uid) + "\",\"id\": " + blocksData[0] + ",\"customName\": \"" + uint8ArrayToChar(name, 8) + "\",\"currentHp\": " + blocksData[9] + ",\"currentLevel\": " + blocksData[10] + ",\"currentXp\": " + blocksData[11] + ",\"height\": " + blocksData[12] + ",\"weight\": " + blocksData[13] + "}";

  Serial.println(result);
}

String uint8ArrayToString(uint8_t* data, size_t length) 
{
    String result = "";
    for (size_t i = 0; i < length; i++) 
    {
        if (i > 0) 
        {
            result += " ";
        }
        // Formatage pour afficher deux chiffres en hexadécimal
        if (data[i] < 16) 
        {
            result += "0";
        }
        result += String(data[i], HEX);
    }
    return result;
}

String uint8ArrayToChar(uint8_t* data, size_t length) 
{
    String result = "";

    for (size_t i = 0; i < length; ++i) {
        result += static_cast<char>(data[i]);
    }

    return result;
}

String TagToString(byte id[4])
{
  String tagId = "";
  for (byte i = 0; i < 4; i++)
    tagId += String(id[i]) + (i < 3 ? "." : "");
  return tagId;
}