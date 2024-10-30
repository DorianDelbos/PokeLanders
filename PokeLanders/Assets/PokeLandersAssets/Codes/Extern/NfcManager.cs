using System;
using System.IO.Ports;

namespace LandersLegends.Extern
{
	public class NfcManager<T>
	{
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
		public T ProcessData(string data)
		{
			try
			{
				return (T)Activator.CreateInstance(typeof(T), data.ToByte());
			}
			catch (Exception)
			{
				return default(T);
			}
		}
	}
}
