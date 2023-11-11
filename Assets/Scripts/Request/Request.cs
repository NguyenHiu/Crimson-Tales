using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Request : MonoBehaviour
{
    public string requestName;
    public int demand;
    public int total;
    public Item item;

    TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = requestName + " (" + total.ToString() + "/" + demand.ToString() + ")";
    }

    public void SetRequestInfo(string _requestName, int _demand, Item _item)
    {
        requestName = _requestName;
        demand = _demand;
        total = 0;
        item = _item;
    }
}
