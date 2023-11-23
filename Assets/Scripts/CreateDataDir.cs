using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreateDataDir : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Application.dataPath + "\\Data"))
            Directory.CreateDirectory(Application.dataPath + "\\Data\\");
    }
}
