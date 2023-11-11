using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCanvas : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Scan();
    }

    void Scan()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas)
        {
            print("detect canvas");
            canvas.worldCamera = GetComponentInParent<Camera>();
        }
    }
}
