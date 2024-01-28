using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMainCanvasAndDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        GameObject x = GameObject.Find("MainCanvas");
        if (x)
            Destroy(x);
    }
}
