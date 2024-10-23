namespace Lander.Gameplay.Type
{
    public enum ElementaryType
    {
        Water,
        Fire,
        Grass,
        Light,
        Dark
    }

    public static class ElementaryTypeUtils
    {
        public static ElementaryType StringToType(string data) => (ElementaryType)System.Enum.Parse(typeof(ElementaryType), data);

        public static ElementaryType[] StringsToTypes(string[] data)
        {
            ElementaryType[] result = new ElementaryType[data.Length];

            for (int i = 0; i < data.Length; i++)
                result[i] = StringToType(data[i]);

            return result;
        }
    }
}
