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
		public bool ProcessData(string data, out T result)
		{
			result = default;

			try
			{
                result = (T)Activator.CreateInstance(typeof(T), data.ToByte());
				return true;
			}
			catch (Exception) { }

            return false;
        }
	}
}
