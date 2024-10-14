using System;
using System.Collections.Concurrent;
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

    private ConcurrentQueue<LanderDataNFC> dataQueue = new ConcurrentQueue<LanderDataNFC>();
    private Lander lander;

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

    private void Start()
    {
        lander = FindAnyObjectByType<Lander>();
    }

    void Update()
    {
        while (dataQueue.TryDequeue(out LanderDataNFC receivedData))
        {
            lander.Data = receivedData;
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
                    string data = stream.ReadLine();
                    Debug.Log("Valeur reçue : " + data);

                    if (data != "-1")
                    {
                        var receivedData = (LanderDataNFC)JsonUtility.FromJson(data, typeof(LanderDataNFC));
                        dataQueue.Enqueue(receivedData);
                    }
                }
            }
            catch (TimeoutException)
            {

            }
            catch (FormatException)
            {

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