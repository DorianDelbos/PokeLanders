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
    uint8_t data[16] = { 2, 'M', 'a', 'r', 'i', 'o', 'B', 'r', 'o', 12, 65, 125, 23, 255, 0x00, 0x00};    
    WriteBlock(1, data);

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

#ifdef DebugMode
  if (readSuccess) 
  {
    Serial.print("tag:");
    nfc.PrintHex(&uid[0], uidLength);
  }
#endif

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