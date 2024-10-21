using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
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
	[SerializeField] private Parity parity = Parity.None;
	[SerializeField] private int baudRate = 9600;
	[SerializeField] private int dataBits = 8;
	[SerializeField] private StopBits stopBits = StopBits.One;
	[SerializeField] private int timeout = 100;

	// Events variables
	public static event Action<LanderDataNFC> onNewNfcDetect;
	public static event Action<LanderDataNFC> onNfcRemove;
	private string lastTagRegister = null;
	private LanderDataNFC lastDataRegister = null;

	// Queue for thread-safe communication
	private Queue<LanderDataNFC> nfcDataQueue = new Queue<LanderDataNFC>();

	// Debug
	public string DebugNfc = "00 00 00 01 01 42 61 72 6E 61 72 64 00 06 20 16 40 60 00 00";

	void Awake()
	{
		// Singleton
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

		// Arduino
		try
		{
			string arduinoPort = GetArduinoPort();
			stream = new SerialPort(arduinoPort, baudRate, parity, dataBits, stopBits) { ReadTimeout = timeout };
			stream.Open();
		}
		catch (Exception e)
		{
			Debug.LogError($"Arduino can't be opened !\n{e}");
		}

		// Multithreading
		try
		{
			readThread = new Thread(UpdateSerialData);
			readThread.Start();
		}
		catch (Exception e)
		{
			Debug.LogError($"Multithreading error !\n{e}");
		}
	}

	private string GetArduinoPort()
	{
		string[] ports = SerialPort.GetPortNames();
		foreach (string port in ports)
		{
			try
			{
				using (stream = new SerialPort(port, baudRate, parity, dataBits, stopBits))
				{
					stream.WriteTimeout = timeout;
					stream.ReadTimeout = timeout;

					stream.Open();
					stream.Write("IsArduino\n");
					Thread.Sleep(1000);
					string response = stream.ReadLine();

					if (response.Contains("TRUE"))
						return port;
				}
			}
			catch (TimeoutException) { }
			finally
			{
				if (stream.IsOpen)
					stream.Close();
			}
		}

		return string.Empty;
	}


	private void UpdateSerialData()
	{
		while (keepReading /*&& stream.IsOpen*/)
		{
			try
			{
				if (nfcDataQueue.Count <= 0)
				{
					//ProcessData(stream.ReadLine());
					ProcessData(DebugNfc);
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
			nfcDataQueue.Enqueue(null);
			return;
		}

		try
		{
			LanderDataNFC receivedData = new LanderDataNFC(StringToByteArray(data));
			nfcDataQueue.Enqueue(receivedData);
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
			nfcData = nfcDataQueue.Dequeue();

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
