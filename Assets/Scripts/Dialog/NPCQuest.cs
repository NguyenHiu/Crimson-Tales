using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuest : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] DialogManager dialogManager;

    void Start()
    {
        if (!dialogManager)
        {
            dialogManager = FindAnyObjectByType<DialogManager>();
        }
    }

    public void Interact()
    {
        print("damn, you are interacting with me!");
        StartCoroutine(dialogManager.ShowDialog(this.dialog));
    }
}
