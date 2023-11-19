using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCRequest : MonoBehaviour, Interactable
{
    [SerializeField] string NPCName;
    [SerializeField] List<Dialog> dialogs;
    [SerializeField] bool containRequest;
    [SerializeField] Transform requestSign;
    [SerializeField] Item rewardItem;
    InventoryManager inventoryManager;
    DialogManager dialogManager;
    DialogState dialogState = DialogState.Waiting;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        if (!dialogManager)
            dialogManager = FindAnyObjectByType<DialogManager>();
        if (!inventoryManager)
            inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    void Update()
    {
        UpdateRequestSign();
    }

    public void Interact()
    {
        audioManager.NPCTalk();
        if (dialogState == DialogState.Processing &&
            dialogManager.TheRequestIsDone(
                dialogs[(int)DialogState.Waiting].Request, false
            ))
        {
            if (!GiveReward())
            {
                StartCoroutine(dialogManager.ShowDialog(
                    "???", dialogs[(int)DialogState.CannotGiveReward],
                    dialogState,
                    (x) => { }
                ));
                return;
            }
            dialogState = DialogState.Complete;
            dialogManager.TheRequestIsDone(
                dialogs[(int)DialogState.Waiting].Request, true
            );
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
                            (newState) =>
                            {
                                dialogState = newState;
                            },
                            accept, reject));
    }

    bool GiveReward()
    {
        return inventoryManager.AddItem(rewardItem);
    }

    void UpdateRequestSign()
    {
        if (dialogState != DialogState.Waiting)
            transform.GetChild(0).gameObject.SetActive(false);
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
    CannotGiveReward = 6,
}