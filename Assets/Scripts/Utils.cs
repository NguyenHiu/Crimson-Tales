using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref: https://discussions.unity.com/t/how-do-you-save-write-and-load-from-a-file/180577/2
public class SAVE
{
    static public void SaveFile(string data, string filePath)
    {
        string destination = Application.dataPath + "\\" + filePath.ToString();
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new();
        bf.Serialize(file, data);
        file.Close();
    }

    static public string LoadFile(string filePath)
    {
        string destination = Application.dataPath + "\\" + filePath.ToString();
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
            return "";

        BinaryFormatter bf = new BinaryFormatter();
        string data = (string)bf.Deserialize(file);
        file.Close();
        return data;
    }

}