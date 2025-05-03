#include <Wire.h>
#include <Adafruit_PN532.h>

#define TagCount 4
#define BlockCount 16
#define DataToSendCount (TagCount + 3 * BlockCount)
#define Debug 0

#define SCK_PIN 2
#define MISO_PIN 5
#define MOSI_PIN 3
#define SS_PIN 4 

// Create an instance of the PN532 using I2C
Adafruit_PN532 nfc(SCK_PIN, MISO_PIN, MOSI_PIN, SS_PIN);

// Define the default key A (6 bytes)
uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

// Data who will be send to computer
uint8_t dataToSend[DataToSendCount] = { 0x00 };

uint8_t blockPosition[5] = {-1,0,1,-1,2};

void setup(void) 
{
  Serial.begin(115200);

  // Initialize the I2C bus and the PN532
  Wire.begin();
  nfc.begin();
  
  uint32_t versiondata = nfc.getFirmwareVersion();
  
  if (!versiondata) 
  {
    Serial.print("Didn't find PN532 module");
    while (1);
  }

  // Print firmware version
  #if Debug
    Serial.print("Found chip PN532 with firmware version: ");
    Serial.print((versiondata >> 24) & 0xFF, DEC); // Major version
    Serial.print(".");
    Serial.print((versiondata >> 16) & 0xFF, DEC); // Minor version
    Serial.print(".");
    Serial.println((versiondata >> 8) & 0xFF, DEC); // Patch version
  #endif

  // Configure the PN532 to read MiFare cards
  nfc.SAMConfig();
  
  #if Debug
    Serial.println("Waiting for RFID/NFC card...");
  #endif
}

void loop(void) 
{
  uint8_t uid[] = { 0, 0, 0, 0, 0, 0, 0 };
  uint8_t uidLength;

  // Check if a card is available to read
  if (readCard(uid, &uidLength)) 
  {
    #if Debug
      Serial.println("Found card with UID: ");
    #endif
    
    for (uint8_t i = 0; i < uidLength; i++) 
    {
      #if Debug
        Serial.print(" 0x");
        Serial.print(uid[i], HEX);
      #endif
    }

    #if Debug
      Serial.println("");
    #endif

    // Store the UID and block data into dataToSend
    storeUID(uid, uidLength);
    storeBlockData(uid, uidLength, 1);
    storeBlockData(uid, uidLength, 2);
    storeBlockData(uid, uidLength, 4);
    
    // Print the stored data (if Debug is enabled)
    #if Debug
      Serial.println("Data stored in dataToSend: ");
      for (uint8_t i = 0; i < DataToSendCount; i++) 
      {
        Serial.print(" 0x");
        Serial.print(dataToSend[i], HEX);
      }
      Serial.println();
    #endif

    // Send data to the computer
    sendDataToComputer();
    
    delay(1000); // Wait for a while before checking for a new card
  } 
  else 
  {
    #if Debug
      Serial.print(".");
    #endif
  }
}

// Function to detect if a card is present and return the UID
bool readCard(uint8_t *uid, uint8_t *uidLength) 
{
  uint8_t success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, uidLength);
  return success;
}

// Function to authenticate and read a specific block
void storeBlockData(uint8_t *uid, uint8_t uidLength, uint8_t block) 
{
  uint8_t data[BlockCount];   // A MiFare block is 16 bytes

  // Authenticate using Key A
  uint8_t success = nfc.mifareclassic_AuthenticateBlock(uid, uidLength, block, 1, keya);

  if (success) 
  {
    #if Debug
      Serial.print("Block ");
      Serial.print(block);
      Serial.println(" read.");
    #endif

    // Now that we are authenticated, read the block
    success = nfc.mifareclassic_ReadDataBlock(block, data);
    
    if (success) 
    {
      // Store the data from the block into dataToSend
      // Each block starts 16 bytes after the previous one
      uint8_t startIdx = TagCount + blockPosition[block] * BlockCount; // First block starts after 4 bytes for the UID

      for (uint8_t i = 0; i < BlockCount; i++) 
      {
        // Check if we have space in the dataToSend array
        if (startIdx + i < DataToSendCount)
        {
          dataToSend[startIdx + i] = data[i];
        }
      }
    } 
    else 
    {
      #if Debug
        Serial.println("Failed to read block!");
      #endif
    }
  } 
  else 
  {
    #if Debug
      Serial.println("Authentication failed!");
    #endif
  }
}

// Function to store the UID into dataToSend
void storeUID(uint8_t *uid, uint8_t uidLength)
{
  // Store the UID into dataToSend starting from the first position
  for (uint8_t i = 0; i < uidLength; i++) 
  {
    if (i < TagCount) // Ensure we don't exceed the space for UID (only 4 bytes for UID)
    {
      dataToSend[i] = uid[i];
    }
  }
}

// Fonction pour envoyer les données sous forme d'un tableau de bytes
void sendDataToComputer()
{
  // Envoyer l'array de bytes tel quel
  Serial.write(dataToSend, DataToSendCount);
}