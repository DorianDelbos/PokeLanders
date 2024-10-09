using System.IO.Ports;
using UnityEngine;

public class ArduinoReader : MonoBehaviour
{
    private SerialPort stream;
    [SerializeField] private string portName = "COM5";
    [SerializeField] private Parity parity = Parity.None;
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private int dataBits = 8;
    [SerializeField] private StopBits stopBits = StopBits.One;

    public SerialPort Stream => stream;

    void Awake()
    {
        stream = new SerialPort();
        stream.PortName = portName;
        stream.Parity = parity;
        stream.DataBits = dataBits;
        stream.StopBits = stopBits;
        stream.BaudRate = baudRate;

        try
        {
            stream.Open();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Arduino can't be open !\n{e}");
        }
    }

    public void OnApplicationQuit()
    {
        stream.Close();
    }
}