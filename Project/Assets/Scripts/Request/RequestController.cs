using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RequestController : MonoBehaviour
{
    [SerializeField] GameObject requestGroupCanvas;
    [SerializeField] Transform requestsCanvas;
    public List<GameObject> requestManagers;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] GameObject requestPrefab;

    [SerializeField] RequestInfo DebugRequestInfo;


    // void Awake()
    // {
    //     requestsCanvas = requestGroupCanvas.transform.GetChild(1);
    // }

    void Update()
    {
        if (requestManagers.Count != 0) requestGroupCanvas.SetActive(true);
        else requestGroupCanvas.SetActive(false);
        UpdateRequestsProcess();
    }

    void UpdateRequestsProcess()
    {
        foreach (GameObject requestManagerGO in requestManagers)
        {
            RequestManager requestManager = requestManagerGO.GetComponent<RequestManager>();
            requestManager
                .UpdateRequestProcess(
                    inventoryManager.NumberOfItem(requestManager.GetSampleItem())
            );
            requestManagerGO.GetComponent<TextMeshProUGUI>().text = requestManager.Format();
        }
    }

    public bool IsRequestDone(RequestInfo requestInfo, bool removeIfDone)
    {
        foreach (GameObject requestManagerGO in requestManagers)
        {
            RequestManager requestManager = requestManagerGO.GetComponent<RequestManager>();
            if (requestManager.IsEqual(requestInfo))
            {
                if (!requestManager.IsDone())
                    return false;

                if (removeIfDone)
                {
                    GameObject _requestMangerGO = requestManagerGO;
                    requestManagers.Remove(_requestMangerGO);
                    Destroy(_requestMangerGO);
                }
                return true;
            }
        }
        return false;
    }

    public void AddNewRequest(RequestInfo requestInfo)
    {
        print("Add new request in reuqest controller");
        GameObject newRequestOB = Instantiate(requestPrefab);
        newRequestOB.transform.SetParent(requestsCanvas);
        newRequestOB.transform.position =
            new Vector3(newRequestOB.transform.position.x,
                        newRequestOB.transform.position.y,
                        0);
        newRequestOB.transform.localScale = new Vector3(1, 1, 1);
        RequestManager newRequest = newRequestOB.GetComponent<RequestManager>();
        newRequest.InitRequest(requestInfo);
        requestManagers.Add(newRequestOB);
    }

    public void DebugAddRequest()
    {
        AddNewRequest(DebugRequestInfo);
    }
}