using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogGroup;
    [SerializeField] GameObject buttonGroup;
    [SerializeField] RequestController requestController;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] HeroController heroController;
    [SerializeField] Text textBox;
    [SerializeField] int wordSpeed;

    int currentLine = 0;
    string NPCName;
    Dialog dialog, accept, reject;
    bool isTyping = false;
    bool waitingForRequest = false;
    Action<DialogState> onUpdatedState;
    DialogState dialogState;

    public IEnumerator ShowDialog(string NPCName, Dialog dialog, DialogState currState, Action<DialogState> onUpdated, Dialog acceptedDialog = null, Dialog rejectedDialog = null)
    {
        yield return new WaitForEndOfFrame();
        heroController.SetDialogOn();
        dialogGroup.SetActive(true);
        onUpdatedState = onUpdated;
        this.dialog = dialog;
        this.accept = acceptedDialog;
        this.reject = rejectedDialog;
        this.dialogState = currState;
        this.NPCName = NPCName;
        StartCoroutine(Typing(this.dialog.Lines[currentLine++]));
    }

    // 
    public bool TheRequestIsDone(RequestInfo requestInfo)
    {
        return requestController.IsRequestDone(requestInfo, true);
    }

    void Update()
    {
        if (dialogGroup.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse1) && !isTyping && !waitingForRequest)
        {
            if (currentLine < this.dialog.Lines.Count)
                StartCoroutine(Typing(dialog.Lines[currentLine++]));
            else
                ShutdownDialog();
        }
    }

    void ShutdownDialog()
    {
        if (dialogState == DialogState.Complete)
            onUpdatedState(DialogState.None);
        dialogGroup.SetActive(false);
        heroController.SetDialogOff();
        currentLine = 0;
    }

    public IEnumerator Typing(string line)
    {
        isTyping = true;
        textBox.text = "<b>" + NPCName.ToString() + "</b>: ";
        foreach (char c in line)
        {
            textBox.text += c;
            yield return new WaitForSeconds(1f / wordSpeed);
        }
        if (dialog.Request.IsValidRequestInfo() && currentLine == dialog.Lines.Count)
        {
            waitingForRequest = true;
            buttonGroup.SetActive(true);
        }
        isTyping = false;
    }

    public void OnAcceptButtonClick()
    {
        requestController.AddNewRequest(dialog.Request);
        onUpdatedState(DialogState.Processing);
        dialog = accept;
        currentLine = 0;
        buttonGroup.SetActive(false);
        waitingForRequest = false;
        StartCoroutine(Typing(dialog.Lines[currentLine++]));
    }

    public void OnRejectButtonClick()
    {
        dialog = reject;
        currentLine = 0;
        buttonGroup.SetActive(false);
        waitingForRequest = false;
        StartCoroutine(Typing(dialog.Lines[currentLine++]));
    }
}
