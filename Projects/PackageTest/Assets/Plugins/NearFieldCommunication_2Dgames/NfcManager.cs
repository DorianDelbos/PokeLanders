using System;
#if NET_4_6
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
                result = (T)Activator.CreateInstance(typeof(T), StringToByte(data));
				return true;
			}
			catch (Exception) { }

            return false;
        }

        private static byte[] StringToByte(string _string)
        {
            _string = _string.Replace(" ", "");
            byte[] bytes = new byte[_string.Length / 2];

            for (int i = 0; i < _string.Length; i += 2)
                bytes[i / 2] = byte.Parse(_string.Substring(i, 2), NumberStyles.HexNumber);

            return bytes;
        }
#else
		public NfcManager()
		{
            throw new InvalidOperationException("NFC functionality is only available with the .NET Framework API Compatibility Level in Unity. Please change the API Compatibility Level to \".NET Framework\" in your Unity project settings.");
        }
#endif
    }
}
