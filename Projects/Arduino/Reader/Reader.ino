#include <Wire.h>
#include <Adafruit_PN532.h>

#define SCK_PIN 2
#define MISO_PIN 5
#define MOSI_PIN 3
#define SS_PIN 4

#define Debug 0

Adafruit_PN532 nfc(SCK_PIN, MISO_PIN, MOSI_PIN, SS_PIN);

uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

void setup() 
{
  Serial.begin(115200);
  Wire.begin();
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  if (!versiondata) 
  {
    #if Debug
    Serial.println("ERROR:PN532_NOT_FOUND");
    #endif
    while (1);
  }

  nfc.SAMConfig();

  #if Debug
  Serial.println("Ready for commands...");
  #endif
}

void loop() 
{
  if (Serial.available()) 
  {
    String command = Serial.readStringUntil('\n');
    command.trim();

    #if Debug
    Serial.println(command);
    #endif

    if (command == "READ_TAG") 
    {
      readTagCommand();
    } 
    else if (command.startsWith("READ_BLOCK:")) 
    {
      handleReadBlockCommand(command);
    }
  }
}

void readTagCommand() 
{
  uint8_t uid[7];
  uint8_t uidLength;

  if (nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, &uidLength)) 
  {
    #if Debug
    Serial.print("UID: ");
    for (int i = 0; i < uidLength; i++) 
    {
      Serial.print("0x");
      if (uid[i] < 0x10) {
        Serial.print("0");
      }
      Serial.print(uid[i], HEX);
      if (i < uidLength - 1) {
        Serial.print(":");
      }
    }
    Serial.println();
    #else
    Serial.write(uid, uidLength);
    #endif
  } 
  #if Debug
  else 
  {
    Serial.println("NO_TAG");
  }
  #endif
}

void handleReadBlockCommand(String cmd) 
{
  int block = -1;
  int sector = -1;
  
  // Parse the command to extract block and sector
  sscanf(cmd.c_str(), "READ_BLOCK:%d:%d", &block, &sector);

  uint8_t uid[7];
  uint8_t uidLength;

  // Check if there's a tag present
  if (!nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, &uidLength)) 
  {
    #if Debug
    Serial.println("NO_TAG");
    #endif
    return;
  }

  // Authenticate the sector (sector number * 4 is the first block in that sector)
  int blockStart = sector * 4;  // The first block of the given sector
  if (nfc.mifareclassic_AuthenticateBlock(uid, uidLength, blockStart, 1, keya)) 
  {
    uint8_t data[16];

    // Make sure the requested block is in the sector
    if (blockStart + block >= 0 && blockStart + block < 64)  // Check if block is valid
    {
      // Read the data from the specified block
      if (nfc.mifareclassic_ReadDataBlock(blockStart + block, data)) 
      {
        #if Debug
        Serial.print("Data Block: ");
        for (int i = 0; i < 16; i++) 
        {
          Serial.print("0x");
          if (data[i] < 0x10) {
            Serial.print("0");
          }
          Serial.print(data[i], HEX);
          if (i < 15) {
            Serial.print(":");
          }
        }
        Serial.println();
        #else
        Serial.write(data, 16);  // Write the data back to Serial output
        #endif
      } 
      #if Debug
      else 
      {
        Serial.println("READ_FAIL");
      }
      #endif
    }
    else 
    {
      #if Debug
      Serial.println("INVALID_BLOCK");
      #endif
    }
  } 
  #if Debug
  else 
  {
    Serial.println("AUTH_FAIL");
  }
  #endif
}
