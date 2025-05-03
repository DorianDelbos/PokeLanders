#if PLATFORM_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
        #region SERIAL PORTS

        private static SerialPort serialPort;

        // Open serial communication with the Arduino
        public static void OpenSerialPort(string portName, int baudRate = 9600)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort(portName, baudRate)
                {
                    ReadTimeout = 500
                };
            }

            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    Debug.Log("Serial port opened successfully.");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.LogError("Access denied to the serial port: " + ex.Message);
                }
                catch (IOException ex)
                {
                    Debug.LogError("IOException: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.LogError("An error occurred while opening the serial port: " + ex.Message);
                }
            }
        }

        // Close serial communication with the Arduino
        public static void CloseSerialPort()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                Debug.Log("Serial port closed.");
            }
        }

        #endregion

        #region MAIN

        // Read the NFC tag (sends a command to read the NFC tag)
        public static async Task<byte[]> ReadTagAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Send a command to the Arduino to read the tag
                serialPort.WriteLine("READ_TAG");  // Arduino command to read the tag

                Thread.Sleep(100);
                return await Task.Run(() => ReadSerialData());
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading the tag: " + ex.Message);
                return null;
            }
        }

        // Read a specific block (sends a command to read a specific block)
        public static async Task<byte[]> ReadBlockAsync(int block, int sector, CancellationToken cancellationToken)
        {
            try
            {
                // Build the command to read a specific block
                serialPort.WriteLine($"READ_BLOCK:{block}:{sector}");  // Send the command to the Arduino

                Thread.Sleep(100);
                return await Task.Run(() => ReadSerialData());
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading the block: " + ex.Message);
                return null;
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task<byte[]> WriteBlockAsync(int block, int sector, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        #endregion

        #region COMMON

        // Read data from the serial port asynchronously
        private static byte[] ReadSerialData()
        {
            List<byte> data = new List<byte>();
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    int readByte = serialPort.ReadByte();
                    data.Add((byte)readByte);

                    // Prevent tight CPU loop
                    Thread.Sleep(100);
                }
            }
            catch (TimeoutException)
            {
                Debug.LogWarning("Timeout");
            }
            catch (IOException ioEx)
            {
                Debug.LogError("IO Exception: " + ioEx.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while reading data: " + ex.Message);
            }

            return data.ToArray();
        }

        #endregion
    }
}
#endif
