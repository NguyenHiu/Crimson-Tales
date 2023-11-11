using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCamera : MonoBehaviour
{
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        Scan();
    }

    void Scan()
    {
        if (!canvas.worldCamera)
        {
            Camera camera = FindAnyObjectByType<Camera>();
            if (camera)
            {
                print("detect camera");
                canvas.worldCamera = camera;
            }
        }
    }
}
