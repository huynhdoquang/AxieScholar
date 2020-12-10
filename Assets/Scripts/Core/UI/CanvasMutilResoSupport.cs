using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Galaxy.UI.NewUI
{
    public class CanvasMutilResoSupport : MonoBehaviour
    {
        private UnityEngine.UI.CanvasScaler canvasScaler;

        private void Awake()
        {
            if (canvasScaler == null)
                canvasScaler = GetComponent<UnityEngine.UI.CanvasScaler>();
            var ratio = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
            var physicRatio = Screen.width / (float)Screen.height;
            int match = physicRatio >= ratio ? 1 : 0;
            canvasScaler.matchWidthOrHeight = match;
        }
    }
}