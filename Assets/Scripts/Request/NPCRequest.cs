using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRequest : MonoBehaviour, Interactable
{
    [SerializeField] string NPCName;
    [SerializeField] List<Dialog> dialogs;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] bool containRequest;
    DialogState dialogState = DialogState.Waiting;

    void Start()
    {
        if (!dialogManager)
            dialogManager = FindAnyObjectByType<DialogManager>();
    }

    public void Interact(HeroController hero)
    {
        if (dialogState == DialogState.Processing &&
            dialogManager.TheRequestIsDone(
                dialogs[(int)DialogState.Waiting].Request
            ))
        {
            dialogState = DialogState.Complete;
        }

        Dialog accept = null, reject = null;
        if (dialogState == DialogState.Waiting && containRequest)
        {
            accept = dialogs[(int)DialogState.PlayerAccept];
            reject = dialogs[(int)DialogState.PlayerReject];
        }

        StartCoroutine(dialogManager.ShowDialog(
                            NPCName, dialogs[(int)dialogState],
                            dialogState,
                            (newState) => { dialogState = newState; },
                            accept, reject));
    }

    public void GoToNextState()
    {
        dialogState += 1;
    }

    public string MyToString()
    {
        string s = "";
        s += NPCName + "," + (int)dialogState;
        return s;
    }

    public void LoadData(DialogState dialogState)
    {
        this.dialogState = dialogState;
    }

    public string Name { get { return NPCName; } }
}


public enum DialogState
{
    Waiting = 0,
    Processing = 1,
    Complete = 2,
    PlayerAccept = 3,
    PlayerReject = 4,
    None = 5,
}