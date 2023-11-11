using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public List<Request> requests;
    public InventoryManager inventoryManager;
    public Item healthItem;
    public GameObject requestPrefab;

    void Start()
    {
        GameObject newRequestOB = Instantiate(requestPrefab, transform);
        Request newRequest = newRequestOB.GetComponent<Request>();
        newRequest.SetRequestInfo("Collect health potion", 5, healthItem);
        requests.Add(newRequest);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Request request in requests)
        {
            int playerHas = inventoryManager.NumberOfItem(request.item);
            request.total = playerHas;
            if (request.demand <= playerHas)
            {
                print("a request is done");
            }
        }
    }

    // public void NewRequest(Item item, int demand)
    // {
    //     requests.Add(new Request(item, demand));
    // }
}