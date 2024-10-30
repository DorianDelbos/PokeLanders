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

void setup(void) 
{
  Serial.begin(9600);
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  
  if (!versiondata) 
  {
    Serial.println("Didn't Find PN53x Module");
    while (1); // Halt
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
    uint8_t dataBlock1[16] = { 'S', 'M', 'a', 'r', 'i', 'o', 'B', 'r', 'o', 0x00, 0x00, 0x00, 0x00, 0x00, 0x50, 0x05};
    WriteBlock(1, dataBlock1);
    uint8_t dataBlock2[16] = { 0x00, 0x03, 0x00, 0x0A, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x0F, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x02};
    WriteBlock(2, dataBlock2);
    uint8_t dataBlock3[16] = { 0x00, 0x03, 0x00, 0x04, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06};
    WriteBlock(4, dataBlock3);

    delay(1000);    
    Serial.println("Waiting for a card ...");
  }
  else
  {
    Serial.println("Tag not detect !");
  }
}

bool tagIsDetect() 
{
  uint8_t uidRead[8] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
  bool readSuccess = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, &uidRead[0], &uidLength);

  if (memcmp(uidRead, uid, sizeof(uid)) == 0) {
    return false;
  }

  memcpy(uid, uidRead, sizeof(uid));

  if (readSuccess) 
  {
    Serial.print("tag:");
    nfc.PrintHex(&uid[0], uidLength);
  }
  return readSuccess;
}

bool WriteBlock(uint8_t blockNumber, uint8_t *data)
{
    uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
    if (!nfc.mifareclassic_AuthenticateBlock(uid, uidLength, blockNumber, 1, keya))
    {
      Serial.println("Failed to authenticate block !");
      return false;
    }
    
    if (!nfc.mifareclassic_WriteDataBlock(blockNumber, data))
    {
      Serial.println("Failed to write data block !");
      return false;
    }

    return true;
}