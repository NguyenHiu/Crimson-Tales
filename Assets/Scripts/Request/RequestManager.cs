using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct RequestInfo
{
    public string name;
    public int demand;
    public Item sampleItem;

    public RequestInfo(RequestInfo other)
    {
        this.name = other.name;
        this.demand = other.demand;
        this.sampleItem = other.sampleItem;
    }

    public readonly bool IsValidRequestInfo()
    {
        return this.name != "";
    }
}

public class RequestManager : MonoBehaviour
{
    [SerializeField] RequestInfo requestInfo;
    int total;

    public void InitRequest(RequestInfo _requestInfo)
    {
        total = 0;
        requestInfo = _requestInfo;
    }

    public Item GetSampleItem()
    {
        return requestInfo.sampleItem;
    }

    public string Format()
    {
        return requestInfo.name + " " +
               total + "/" + requestInfo.demand + ")";
    }

    public void UpdateRequestProcess(int newTotal)
    {
        total = newTotal;
    }

    public bool IsEqual(RequestInfo rInfo)
    {
        return rInfo.name == requestInfo.name;
    }

    public bool IsDone()
    {
        return total >= requestInfo.demand;
    }
}
