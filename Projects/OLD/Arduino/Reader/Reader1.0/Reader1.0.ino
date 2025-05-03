#define TagCount 4
#define BlockCount 16
#define DataToSendCount 52
#define GreenLED 12
#define RedLED 13

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
uint8_t blocksToRead[3] = { 1, 2, 4 };
String lastMessageSend = "-1";

void setup(void) 
{
  Serial.begin(9600);

  // LED config
  pinMode(RedLED, OUTPUT);
  pinMode(GreenLED, OUTPUT);

  // Nfc config
  nfc.begin();

  while (!TestModule()) { delay(1000); }
}

void loop() 
{
  while (!TestModule()) { delay(1000); }

  // Data send from Unity to Arduino
  ProcessMessage();

  // If tag is detect, and stock it in data to send
  if (nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, dataToSend, &uidLength))
  {
    // Read blocks
    int count = 0;
    for (uint8_t block : blocksToRead)
    {
      delay(100);
      nfc.mifareclassic_AuthenticateBlock(dataToSend, uidLength, block, 1, keya);
      delay(100);
      nfc.mifareclassic_ReadDataBlock(block, &dataToSend[TagCount + BlockCount * count++]);
    }

    // Send data to Unity
    SendMessage(uint8ArrayToString(dataToSend, DataToSendCount), true);
  }
  
  delay(500);
}

//////////////////////////////////////////
// Send a message from Arduino to Unity //
//////////////////////////////////////////
bool TestModule()
{
  if (nfc.getFirmwareVersion())
  {
    digitalWrite(GreenLED, HIGH);
    digitalWrite(RedLED, LOW);

    nfc.SAMConfig();

    return true;
  }
  else
  {
    digitalWrite(GreenLED, LOW);
    digitalWrite(RedLED, HIGH);

    return false;
  }
}

void SendMessage(String messageToSend, bool registerLastMessage)
{
  if (registerLastMessage)
  {
    if (lastMessageSend != messageToSend)
    {
      Serial.println(messageToSend);
      Serial.flush();
      lastMessageSend = messageToSend;
    }
  }
  else
    Serial.println(messageToSend);
    Serial.flush();
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

//////////////////////////////////////////
// Send a message from Unity to Arduino //
//////////////////////////////////////////
void ProcessMessage()
{
  if (Serial.available() > 0) 
  {
    String message = Serial.readStringUntil('\n');
    //SendMessage(message, false);

    if (message.equals("IsLanderPortal"))
      SendMessage("TRUE", false);
  }
}