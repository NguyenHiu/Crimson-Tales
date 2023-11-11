using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject DialogGroup;
    [SerializeField] Text textBox;
    [SerializeField] int wordSpeed;

    int currentLine = 0;
    Dialog dialog;
    bool isTyping = false;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        FindAnyObjectByType<HeroController>().dialogOn = true;
        DialogGroup.SetActive(true);
        this.dialog = dialog;
        StartCoroutine(Typing(this.dialog.Lines[currentLine++]));
    }

    void Update()
    {
        if (DialogGroup.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse1) && !isTyping)
        {
            if (currentLine < this.dialog.Lines.Count)
                StartCoroutine(Typing(dialog.Lines[currentLine++]));
            else
            {
                DialogGroup.SetActive(false);
                FindAnyObjectByType<HeroController>().dialogOn = false;
                currentLine = 0;
            }
        }
    }

    public IEnumerator Typing(string line)
    {
        isTyping = true;
        textBox.text = "";
        foreach (char c in line)
        {
            textBox.text += c;
            yield return new WaitForSeconds(1f / wordSpeed);
        }
        isTyping = false;
    }
}
