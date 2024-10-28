using LandersLegends.Gameplay;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;


namespace LandersLegends.Extern
{
	public class ExternLanderManager : MonoBehaviour
	{
		// Instance
		public static ExternLanderManager instance;

		// Stream and threading
		private SerialPort portalStream = null;
		private Thread readThread;
		private bool keepReading = false;

		// Events
		public static event Action onPortalConnect;
		public static event Action onPortalDisconnect;
		public static event Action<LanderDataNFC> onLanderDetect;
		public static event Action<LanderDataNFC> onLanderRemove;

		// Others
		private NfcManager<LanderDataNFC> nfcManagerLander;
		private LanderDataNFC lastDataRegister = null;
		private string lastTagRegister = null;

		// Getters
		public bool PortalIsConnected => portalStream != null;

		private void Awake()
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
		}

		private void Start()
		{
			// Start all serials events
			SerialManager.Initialize();
			// Initialize nfc
			nfcManagerLander = new NfcManager<LanderDataNFC>();
		}

		private void OnEnable()
		{
			// Subscribe SerialManager events
			SerialManager.OnSerialPortDetect += CheckSerialIsLander;
			SerialManager.OnSerialPortRemove += CheckPortalDisconnect;
		}

		private void OnDisable()
		{
			// Unsubscribe SerialManager events
			SerialManager.OnSerialPortDetect -= CheckSerialIsLander;
			SerialManager.OnSerialPortRemove -= CheckPortalDisconnect;
		}

		void Update()
		{
			while (nfcManagerLander.nfcDataQueue.Count > 0)
			{
				LanderDataNFC nfcData;
				nfcData = nfcManagerLander.nfcDataQueue.Dequeue();

				if (nfcData == null)
				{
					if (lastTagRegister != null)
					{
						lastTagRegister = null;
						onLanderRemove?.Invoke(lastDataRegister);
					}
				}
				else
				{
					if (lastTagRegister != nfcData.tag)
					{
						lastTagRegister = nfcData.tag;
						lastDataRegister = nfcData;

						onLanderDetect?.Invoke(nfcData);
					}
				}
			}
		}

		public void OnApplicationQuit()
		{
			StopThreading();
			if (portalStream != null && portalStream.IsOpen)
				portalStream.Close();
		}

		#region THREADING
		private void StartThreading()
		{
			try
			{
				keepReading = true;
				readThread = new Thread(UpdateThread);
				readThread.Start();
			}
			catch (Exception e)
			{
				keepReading = false;
				Debug.LogError($"Multithreading error !\n{e}");
			}
		}

		private void StopThreading()
		{
			keepReading = false;
			if (readThread != null && readThread.IsAlive)
			{
				readThread.Join();
			}
		}

		private void UpdateThread()
		{
			while (keepReading)
			{
				string data = nfcManagerLander.ReadNFC(portalStream);
				nfcManagerLander.ProcessData(data);
			}
		}
		#endregion

		#region PORTALS
		private void CheckSerialIsLander(SerialPort serialPort)
		{
			if (portalStream == null)
				return;

			try
			{
				serialPort.Open();
				serialPort.Write("IsArduino\n");
				Thread.Sleep(1000);
				string response = serialPort.ReadLine();

				if (response.Contains("TRUE"))
				{
					// Portal Connected
					portalStream = serialPort;
					onPortalConnect?.Invoke();
					StartThreading();
				}
			}
			catch (TimeoutException) { }
			finally
			{
				if (serialPort.IsOpen)
					serialPort.Close();
			}
		}

		private void CheckPortalDisconnect(SerialPort serialPort)
		{
			if (serialPort.PortName == portalStream.PortName)
			{
				StopThreading(); // Stop threading before close stream !
				portalStream.Close();
				portalStream = null;
				onPortalDisconnect?.Invoke();
			}
		}
		#endregion

#if UNITY_EDITOR
		#region DEBUG
		public void ProccessLanderDebug(string data)
		{
			nfcManagerLander.ProcessData(data);
		}
		#endregion
#endif
	}
}
