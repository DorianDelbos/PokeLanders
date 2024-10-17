using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class NfcModule : MonoBehaviour
{
	public static NfcModule instance;

	// Port and thread
	private SerialPort stream;
	private Thread readThread;
	private bool keepReading = true;

	// Port data
	[SerializeField] private string portName = "COM5";
	[SerializeField] private Parity parity = Parity.None;
	[SerializeField] private int baudRate = 9600;
	[SerializeField] private int dataBits = 8;
	[SerializeField] private StopBits stopBits = StopBits.One;
	[SerializeField] private int readTimeout = 100;

	// Events variables
	public static event Action<LanderDataNFC> onNewNfcDetect;
	public static event Action<LanderDataNFC> onNfcRemove;
	private string lastTagRegister = null;
	private LanderDataNFC lastDataRegister = null;

	// Queue for thread-safe communication
	private Queue<LanderDataNFC> nfcDataQueue = new Queue<LanderDataNFC>();
	private object queueLock = new object();

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
        }
		else
		{
			Destroy(gameObject);
			return;
		}

		stream = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
		{
			ReadTimeout = readTimeout
		};

		try
		{
			stream.Open();
			readThread = new Thread(UpdateSerialData);
			readThread.Start();
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Arduino can't be opened!\n{e}");
		}
	}

	private void UpdateSerialData()
	{
		while (keepReading && stream.IsOpen)
		{
			try
			{
				if (nfcDataQueue.Count <= 0)
				{
					ProcessData(stream.ReadLine());
				}
			}
			catch (TimeoutException) { }
			catch (FormatException) { }
		}
	}

	private void ProcessData(string data)
	{
		if (data == "-1")
		{
			lock (queueLock)
			{
				nfcDataQueue.Enqueue(null);
			}
			return;
		}

		try
        {
            LanderDataNFC receivedData = LanderDataNFC.FromByteArray(StringToByteArray(data));
			lock (queueLock)
			{
				nfcDataQueue.Enqueue(receivedData);
			}
		}
		catch (Exception e)
		{
			Debug.LogWarning($"Card data corrupted !\n{e}");
		}
    }
    public static byte[] StringToByteArray(string hex)
    {
        hex = hex.Replace(" ", "");
        byte[] bytes = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length; i += 2)
            bytes[i / 2] = byte.Parse(hex.Substring(i, 2), NumberStyles.HexNumber);

        return bytes;
    }

    void Update()
	{
		while (nfcDataQueue.Count > 0)
		{
			LanderDataNFC nfcData;
			lock (queueLock)
			{
				nfcData = nfcDataQueue.Dequeue();
			}

			if (nfcData == null)
			{
				if (lastTagRegister != null)
				{
					lastTagRegister = null;
					onNfcRemove?.Invoke(lastDataRegister);
				}
			}
			else
			{
				if (lastTagRegister != nfcData.tag)
				{
					lastTagRegister = nfcData.tag;
					lastDataRegister = nfcData;

					onNewNfcDetect?.Invoke(nfcData);
				}
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
