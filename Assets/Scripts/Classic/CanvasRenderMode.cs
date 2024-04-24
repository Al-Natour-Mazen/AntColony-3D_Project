using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRenderMode : MonoBehaviour
{
    public Camera renderCamera;

    void Start()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = renderCamera;
    }
}
