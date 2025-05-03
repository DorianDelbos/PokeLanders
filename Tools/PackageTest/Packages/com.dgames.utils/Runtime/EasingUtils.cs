using System;

namespace dgames.Utils
{
    public static class EasingUtils
    {
        #region Quadratic Easing

        /// <summary>
        /// Ease In (Quadratic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        /// <summary>
        /// Ease Out (Quadratic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutQuad(float t)
        {
            return -t * (t - 2);
        }

        /// <summary>
        /// Ease In-Out (Quadratic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutQuad(float t)
        {
            if (t < 0.5f)
                return 2 * t * t;
            else
                return -2 * t * t + 4 * t - 1;
        }

        #endregion

        #region Cubic Easing

        /// <summary>
        /// Ease In (Cubic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        /// <summary>
        /// Ease Out (Cubic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutCubic(float t)
        {
            float f = (t - 1);
            return f * f * f + 1;
        }

        /// <summary>
        /// Ease In-Out (Cubic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutCubic(float t)
        {
            if (t < 0.5f)
                return 4 * t * t * t;
            else
            {
                float f = (2 * t - 2);
                return 0.5f * f * f * f + 1;
            }
        }

        #endregion

        #region Quartic Easing

        /// <summary>
        /// Ease In (Quartic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInQuart(float t)
        {
            return t * t * t * t;
        }

        /// <summary>
        /// Ease Out (Quartic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutQuart(float t)
        {
            float f = (t - 1);
            return 1 - f * f * f * f;
        }

        /// <summary>
        /// Ease In-Out (Quartic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutQuart(float t)
        {
            if (t < 0.5f)
                return 8 * t * t * t * t;
            else
            {
                float f = (t - 1);
                return 1 - 8 * f * f * f * f;
            }
        }

        #endregion

        #region Quintic Easing

        /// <summary>
        /// Ease In (Quintic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInQuint(float t)
        {
            return t * t * t * t * t;
        }

        /// <summary>
        /// Ease Out (Quintic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutQuint(float t)
        {
            float f = (t - 1);
            return 1 + f * f * f * f * f;
        }

        /// <summary>
        /// Ease In-Out (Quintic).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutQuint(float t)
        {
            if (t < 0.5f)
                return 16 * t * t * t * t * t;
            else
            {
                float f = (2 * t - 2);
                return 0.5f * f * f * f * f * f + 1;
            }
        }

        #endregion

        #region Sine Easing

        /// <summary>
        /// Ease In (Sine).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInSine(float t)
        {
            return 1 - (float)Math.Cos(t * (Math.PI / 2));
        }

        /// <summary>
        /// Ease Out (Sine).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutSine(float t)
        {
            return (float)Math.Sin(t * (Math.PI / 2));
        }

        /// <summary>
        /// Ease In-Out (Sine).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutSine(float t)
        {
            return 0.5f * (1 - (float)Math.Cos(Math.PI * t));
        }

        #endregion

        #region Exponential Easing

        /// <summary>
        /// Ease In (Exponential).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInExpo(float t)
        {
            return (float)Math.Pow(2, 10 * (t - 1));
        }

        /// <summary>
        /// Ease Out (Exponential).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseOutExpo(float t)
        {
            return 1 - (float)Math.Pow(2, -10 * t);
        }

        /// <summary>
        /// Ease In-Out (Exponential).
        /// </summary>
        /// <param name="t">The time value (from 0 to 1).</param>
        /// <returns>The eased value.</returns>
        public static float EaseInOutExpo(float t)
        {
            if (t < 0.5f)
                return 0.5f * (float)Math.Pow(2, 20 * t - 10);
            else
                return 1 - 0.5f * (float)Math.Pow(2, -20 * t + 10);
        }

        #endregion
    }

}
