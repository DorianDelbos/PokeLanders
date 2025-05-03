using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LandersLegends.Extern
{
	public static class SerialManager
	{
		// Attributes
		private static Parity parity = Parity.None;
		private static int baudRate = 9600;
		private static int dataBits = 8;
		private static StopBits stopBits = StopBits.One;
		private static int timeout = 100;
		private static int checkDelay = 2000;

		private static Dictionary<string, SerialPort> portsDetects = new Dictionary<string, SerialPort>();
		private static Thread checkThread;
		private static bool isRunning = false; // For stopping the thread safely

		// Events
		public static event Action<SerialPort> OnSerialPortDetect;
		public static event Action<SerialPort> OnSerialPortRemove;

		// Static initialization method
		public static void Initialize()
		{
#if UNITY_EDITOR
			OnSerialPortDetect += serial => Debug.Log($"New serial port detected on {serial.PortName}");
			OnSerialPortRemove += serial => Debug.Log($"Serial port removed on {serial.PortName}");
#endif

			// Start checking ports asynchronously
			isRunning = true;
			checkThread = new Thread(CheckPortsAsync);
			checkThread.Start();
		}

		public static void Stop()
		{
			isRunning = false;
			checkThread?.Join();
		}

		private static void CheckPortsAsync()
		{
			while (isRunning)
			{
				string[] ports = SerialPort.GetPortNames();

				// Adding ports not yet detected
				foreach (var port in ports)
				{
					if (!portsDetects.ContainsKey(port))
					{
						SerialPort newSerialPort = new SerialPort(port, baudRate, parity, dataBits, stopBits)
						{
							ReadTimeout = timeout,
							WriteTimeout = timeout
						};
						portsDetects.Add(port, newSerialPort);
						OnSerialPortDetect?.Invoke(newSerialPort);
					}
				}

				// Deleting ports that are no longer detected
				var portsToRemove = portsDetects.Keys.Except(ports).ToList();
				foreach (var port in portsToRemove)
				{
					if (portsDetects.TryGetValue(port, out SerialPort removedPort))
					{
						portsDetects.Remove(port);
						OnSerialPortRemove?.Invoke(removedPort);
					}
				}

				Thread.Sleep(checkDelay);
			}
		}
	}

}
