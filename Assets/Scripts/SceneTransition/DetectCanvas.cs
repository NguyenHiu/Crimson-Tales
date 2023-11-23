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
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas != null) canvas.worldCamera = GetComponentInParent<Camera>();

    }
}
