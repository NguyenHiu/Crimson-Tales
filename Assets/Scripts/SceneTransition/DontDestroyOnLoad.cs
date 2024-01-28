using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using 

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
