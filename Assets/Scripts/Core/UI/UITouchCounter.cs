using UnityEngine;

namespace Net.HungryBug.Core.UI
{
    [DisallowMultipleComponent]
    public class UITouchCounter : UIResolver
    {
        public const float clickDelay = 0.5f;
        private static float lastClick = float.MinValue;

        /// <summary>
        /// Resets interval click time, on show controllers dynamically.
        /// </summary>
        public static void ResetInterval() { lastClick = Time.realtimeSinceStartup; }

        /// <summary>
        /// Check if next touch is available.
        /// </summary>
        public static bool IsNextTouchValid()
        {
            if (Time.realtimeSinceStartup - lastClick >= clickDelay)
            {
                lastClick = Time.realtimeSinceStartup;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
