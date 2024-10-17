#include <SoftwareSerial.h>
#include <PN532_SWHSU.h>
#include <PN532.h>

// Pins variables
SoftwareSerial SWSerial(3, 2);
// Pn532 utils
PN532_SWHSU pn532swhsu(SWSerial);
PN532 nfc(pn532swhsu);

uint8_t uid[8] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
uint8_t uidLength;
uint8_t blocksData[16];

void setup(void) 
{
  Serial.begin(9600);
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  
  if (!versiondata) 
  {
    Serial.println("Didn't Find PN53x Module");
    while(true);
  }
  else
  {
    Serial.println("Pn532 ready, Start NFC Read/Write !");
    Serial.println("Waiting a card ...");
  }
  
  nfc.SAMConfig();
}

void loop() 
{
  if (tagIsDetect())
  {
    ReadBlock(1);
    SendData();

    delay(500);
  }
  else
  {
    Serial.println("-1");
  }
}

bool tagIsDetect() 
{
  bool readSuccess = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uid[0], &uidLength);
  return readSuccess;
}

bool ReadBlock(uint8_t blockNumber)
{
    if (!nfc.mifareclassic_AuthenticateBlock(uid, uidLength, blockNumber, 1, keya))
      return false;

    if (!nfc.mifareclassic_ReadDataBlock(blockNumber, blocksData))
      return false;

    return true;
}

void SendData()
{
  String result = "{\"tag\": \"" + TagToString(uid) + "\",\"id\": " + blocksData[0] + ",\"customName\": \"" + uint8ArrayToChar(&blocksData[1], 8) + "\",\"currentHp\": " + blocksData[9] + ",\"currentLevel\": " + blocksData[10] + ",\"currentXp\": " + blocksData[11] + ",\"height\": " + blocksData[12] + ",\"weight\": " + blocksData[13] + "}";
  Serial.println(result);
}

String uint8ArrayToChar(uint8_t* data, size_t length) 
{
    String result = "";

    for (size_t i = 0; i < length; ++i)
        result += static_cast<char>(data[i]);

    return result;
}

String TagToString(byte id[4])
{
  String tagId = "";

  for (byte i = 0; i < 4; i++)
    tagId += String(id[i]) + (i < 3 ? "." : "");

  return tagId;
}