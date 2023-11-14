using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;
    [SerializeField] RequestInfo requestInfo;
    [SerializeField] List<Item> items;

    public List<string> Lines
    {
        get { return lines; }
    }

    public RequestInfo Request
    {
        get { return requestInfo; }
    }

    public List<Item> Items
    {
        get { return items; }
    }
}
