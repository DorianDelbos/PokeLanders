#define TagCount 4
#define BlockCount 16
#define DataToSendCount 20
#define BlockToRead 1

#include <SoftwareSerial.h>
#include <PN532_SWHSU.h>
#include <PN532.h>

// Pins variables
SoftwareSerial SWSerial(3, 2);
// Pn532 utils
PN532_SWHSU pn532swhsu(SWSerial);
PN532 nfc(pn532swhsu);

uint8_t keya[6] = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
uint8_t uidLength;
uint8_t dataToSend[DataToSendCount] = { 0x00 };
String lastMessageSend = "-1";

void setup(void) 
{
  Serial.begin(9600);

  // Nfc config
  nfc.begin();

  if (!nfc.getFirmwareVersion()) 
    while(true);
  
  nfc.SAMConfig();
}

void loop() 
{
  // If tag is detect, and stock it in data to send
  if (nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, dataToSend, &uidLength))
  {
    // Read 16 bytes of data
    nfc.mifareclassic_AuthenticateBlock(dataToSend, uidLength, BlockToRead, 1, keya);
    nfc.mifareclassic_ReadDataBlock(BlockToRead, &dataToSend[TagCount]);
    // Send data to Unity
    SendMessage(uint8ArrayToString(dataToSend, DataToSendCount));
  }
  else
    SendMessage("-1");
  
  delay(500);
}

void SendMessage(String messageToSend)
{
    if (lastMessageSend != messageToSend)
    {
      Serial.println(messageToSend);
      lastMessageSend = messageToSend;
    }
}

String uint8ArrayToString(uint8_t* data, size_t length) 
{
    String result = "";
    for (size_t i = 0; i < length; i++) 
    {
        if (i > 0) 
            result += " ";
        if (data[i] < 16) 
            result += "0";
        result += String(data[i], HEX);
    }
    return result;
}