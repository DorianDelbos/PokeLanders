namespace Lander.Module.Utilities
{
    public static class LanderUtilities
    {
        public static string GetHeightInInches(int height)
        {
            int kilograms = height / 100;
            int grams = height % 100;

            return $"{kilograms}'{grams:D2}''";
        }

        public static string GetWeightInPounds(int weight)
        {
            return $"{(weight / 100m).ToString("F2")} lbs";
        }

        public static string GetHeightInMeters(int height)
        {
            decimal heightInMeters = height * 0.0254m;
            return heightInMeters.ToString("F2") + " m";
        }

        public static string GetWeightInKilograms(int weight)
        {
            decimal weightInKilograms = weight * 0.453592m;
            return weightInKilograms.ToString("F2") + " kg";
        }

    }
}
