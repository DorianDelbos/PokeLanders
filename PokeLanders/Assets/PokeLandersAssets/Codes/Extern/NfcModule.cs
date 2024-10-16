using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

public class NfcModule : MonoBehaviour
{
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
	public static event Action onNfcRemove;
	private string lastTagRegister = null;

	// Queue for thread-safe communication
	private Queue<LanderDataNFC> nfcDataQueue = new Queue<LanderDataNFC>();
	private object queueLock = new object();

	public string debugDataToSend = $"{{\"tag\": \"00 00 00 00\",\"id\": 3,\"customName\": \"Barnard\",\"currentHp\": 12,\"currentLevel\": 2,\"currentXp\": 20,\"height\": 124,\"weight\": 62}}";

	void Awake()
	{
		stream = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
		{
			ReadTimeout = readTimeout
		};

		try
		{
			//stream.Open();
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
		while (keepReading /*&& stream.IsOpen*/)
		{
			try
			{
				if (nfcDataQueue.Count <= 0)
				{
					//string data = stream.ReadLine();
					string data = debugDataToSend;
					ProcessData(data);
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
				nfcDataQueue.Enqueue(null); // Use null to signal NFC removal
			}
			return;
		}

		var receivedData = JsonUtility.FromJson<LanderDataNFC>(data);
		lock (queueLock)
		{
			nfcDataQueue.Enqueue(receivedData);
		}
	}

	void Update()
	{
		// Process queued NFC data on the main thread
		while (nfcDataQueue.Count > 0)
		{
			LanderDataNFC nfcData;
			lock (queueLock)
			{
				nfcData = nfcDataQueue.Dequeue();
			}

			if (nfcData == null) // Handle NFC removal
			{
				if (lastTagRegister != null)
				{
					lastTagRegister = null;
					onNfcRemove?.Invoke();
				}
			}
			else // Handle new NFC detection
			{
				if (lastTagRegister != nfcData.tag)
				{
					lastTagRegister = nfcData.tag;
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
