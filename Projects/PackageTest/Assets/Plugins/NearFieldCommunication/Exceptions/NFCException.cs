using System;

namespace dgames.nfc
{
    public class NFCException : Exception
    {
        public NFCException(string message) : base(message) { }

        public NFCException(string message, Exception innerException) : base(message, innerException) { }
    }
}
