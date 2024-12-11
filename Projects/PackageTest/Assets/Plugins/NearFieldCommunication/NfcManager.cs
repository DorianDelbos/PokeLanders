using System;
#if NET_4_6
using dgames.Utilities;
using System.Globalization;
using System.IO.Ports;
#endif

namespace dgames.Extern
{
    public class NfcManager<T>
    {
#if NET_4_6
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
#else
		public NfcManager()
		{
            throw new InvalidOperationException("NFC functionality is only available with the .NET Framework API Compatibility Level in Unity. Please change the API Compatibility Level to \".NET Framework\" in your Unity project settings.");
        }
#endif
    }
}
