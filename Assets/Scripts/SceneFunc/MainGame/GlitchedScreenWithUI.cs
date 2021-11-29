using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlitchedScreenWithUI : MonoBehaviour
{
    [SerializeField] private Canvas matchedCanvas;
    [SerializeField] private Camera subCam;

    private void OnEnable()
    {
        matchedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        matchedCanvas.worldCamera = subCam;
    }
    private void OnDisable()
    {
        matchedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        matchedCanvas.worldCamera = Camera.main;
        matchedCanvas.targetDisplay = 0;
    }
}
