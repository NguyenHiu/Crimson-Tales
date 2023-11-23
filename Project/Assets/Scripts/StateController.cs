using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] GameObject rock;
    [SerializeField] string path;

    public void Save()
    {
        if (rock.activeInHierarchy) SAVE.SaveFile("1", path);
        else SAVE.SaveFile("0", path);
    }

    public void Load()
    {
        string rawData = SAVE.LoadFile(path);
        if (rawData == "") return;
        if (rawData == "0") rock.SetActive(false);
        else rock.SetActive(true);
    }
}
