#nullable enable
using System;
using System.Collections;
using UnityEngine;

namespace dgames.Utils
{
    /// <summary>
    /// Utility class that provides time-based methods to manage coroutines with elapsed time.
    /// </summary>
    public static class TimeUtils
    {
        /// <summary>
        /// Coroutine that tracks elapsed time over a given duration, calls a callback every frame with the current elapsed time,
        /// and optionally repeats the routine if specified.
        /// </summary>
        /// <param name="duration">The total time (in seconds) the routine will run before completing.</param>
        /// <param name="onUpdate">The action to be called every frame with the elapsed time as a parameter.</param>
        /// <param name="onComplete">The action to be called when the duration is complete.</param>
        /// <param name="loop">A flag indicating whether the routine should loop after the duration is completed.</param>
        /// <returns>An enumerator that tracks the elapsed time and executes the actions.</returns>
        public static IEnumerator ElapsedTimeRoutine(float duration, Action<float>? onUpdate, Action? onComplete, bool loop = false)
        {
            // Store the time at the start of the routine
            float startTime = Time.time;

            while (true)
            {
                // Track elapsed time
                float elapsedTime = Time.time - startTime;

                // Call the onUpdate action with the elapsed time
                onUpdate?.Invoke(elapsedTime);

                // If the elapsed time exceeds or equals the duration, complete the routine
                if (elapsedTime >= duration)
                {
                    // Call the onComplete action
                    onComplete?.Invoke();

                    // If looping, reset the start time for the next loop
                    if (loop)
                    {
                        startTime = Time.time;
                    }
                    else
                    {
                        break;
                    }
                }

                // Wait for the next frame
                yield return null;
            }
        }
    }
}
