using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoReader : MonoBehaviour
{
    private SerialPort stream;
    private Thread readThread;
    bool keepReading = true;

    [SerializeField] private string portName = "COM5";
    [SerializeField] private Parity parity = Parity.None;
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private int dataBits = 8;
    [SerializeField] private StopBits stopBits = StopBits.One;
    [SerializeField] private int readTimeout = 100;

    public SerialPort Stream => stream;

    void Awake()
    {
        stream = new SerialPort();
        stream.PortName = portName;
        stream.Parity = parity;
        stream.DataBits = dataBits;
        stream.StopBits = stopBits;
        stream.BaudRate = baudRate;
        stream.ReadTimeout = readTimeout;

        try
        {
            stream.Open();
            readThread = new Thread(ReadSerialData);
            readThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Arduino can't be open !\n{e}");
        }
    }

    private void ReadSerialData()
    {
        while (keepReading)
        {
            try
            {
                if (stream.IsOpen)
                {
                    // Lire les données du port série
                    string data = stream.ReadLine();
                    int value = int.Parse(data); // Convertir la chaîne en entier
                    Debug.Log("Valeur reçue : " + value); // Afficher la valeur dans la console
                }
            }
            catch (TimeoutException)
            {
                // Gérer le cas où le délai d'attente se produit
            }
            catch (FormatException)
            {
                // Gérer le cas où la conversion échoue
            }
        }
    }

    public void OnApplicationQuit()
    {
        keepReading = false;
        if (stream.IsOpen)
        {
            stream.Close();
        }
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join();
        }
    }
}