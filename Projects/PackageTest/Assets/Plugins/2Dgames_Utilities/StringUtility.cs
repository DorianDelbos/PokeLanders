namespace dgames.Utilities
{
	public static class StringUtility
	{
		public static byte[] ToByte(this string _string)
		{
			_string = _string.Replace(" ", "");
			byte[] bytes = new byte[_string.Length / 2];

			for (int i = 0; i < _string.Length; i += 2)
				bytes[i / 2] = byte.Parse(_string.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);

			return bytes;
		}
	}
}
