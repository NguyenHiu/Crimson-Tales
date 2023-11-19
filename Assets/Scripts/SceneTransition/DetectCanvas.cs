using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCanvas : MonoBehaviour
{
    void Awake()
    {
        NPCController npccontroller = FindAnyObjectByType<NPCController>();
        if (npccontroller) npccontroller.Load();
        StateController stateController = FindAnyObjectByType<StateController>();
        if (stateController) stateController.Load();
    }

    void Start()
    {
        Scan();
    }

    void Scan()
    {
        Canvas[] canvas = FindObjectsOfType<Canvas>();
        foreach (Canvas c in canvas)
            if (c.name == "MainCanvas")
            {
                c.worldCamera = GetComponentInParent<Camera>();
            }
    }
}
