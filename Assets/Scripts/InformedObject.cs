using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformedObject : MonoBehaviour, Interactable
{
    [SerializeField] string aName;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] Dialog dialog;

    void Start()
    {
        if (!dialogManager)
            dialogManager = FindAnyObjectByType<DialogManager>();
    }

    public void Interact()
    {
        print("Interacting...");
        StartCoroutine(dialogManager.ShowDialog(
            aName, dialog, DialogState.Waiting,
            (newState) => { })
        );
    }


}
