using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] List<NPCRequest> nPCRequests;
    [SerializeField] string path;

    public void Save()
    {
        string data = "";
        for (int i = 0; i < nPCRequests.Count; ++i)
        {
            data += nPCRequests[i].MyToString();
            if (i != nPCRequests.Count - 1)
                data += ";";
        }
        SAVE.SaveFile(data, path);
    }

    public void Load()
    {
        string rawData = SAVE.LoadFile(path);
        if (rawData == "") return;
        List<string> data = rawData.Split(";").ToList();
        for (int i = 0; i < data.Count; ++i)
        {
            string[] splitedData = data[i].Split(",");
            for (int j = 0; j < nPCRequests.Count; ++j)
            {
                if (nPCRequests[i].Name == splitedData[0])
                {
                    nPCRequests[i].LoadData((DialogState)int.Parse(splitedData[1]));
                    data.RemoveAt(i);
                    i = 0;
                    break;
                }
            }
        }
    }
}
