using UnityEngine;

namespace dgames.Utils
{
    public static class MathUtils
    {
        public static float Sigmoid(float x, float force = 1.0f)
        {
            return 1.0f / (1 + Mathf.Exp(-force * x));
        }
    }
}
