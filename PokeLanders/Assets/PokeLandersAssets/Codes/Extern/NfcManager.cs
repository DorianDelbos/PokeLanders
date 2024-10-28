using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace LandersLegends.Extern
{
	public class NfcManager<T>
	{
		public Queue<T> nfcDataQueue = new Queue<T>();

		public string ReadNFC(SerialPort serialPort)
		{
			try
			{
				return serialPort.ReadLine();
			}
			catch (TimeoutException) { }
			catch (FormatException) { }

			return string.Empty;
		}


		public void ProcessData(string data)
		{
			try
			{
				T receivedData = (T)Activator.CreateInstance(typeof(T), data.ToByte());
				nfcDataQueue.Enqueue(receivedData);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Data corrupted !\n{e}");
			}
		}
	}
}
