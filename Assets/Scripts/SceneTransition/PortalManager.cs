using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public int toSceneIndex;
    public Vector2 toPosition;
    MyTransition myTransition;

    void Start()
    {
        myTransition = FindAnyObjectByType<Canvas>().transform.Find("SceneTransitionAnimator").GetComponent<MyTransition>();
        myTransition.ActiveStartTransition();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MyLoadScene());
        }
    }

    IEnumerator MyLoadScene()
    {
        myTransition.ActiveEndTransition();
        NPCController npccontroller = FindAnyObjectByType<NPCController>();
        if (npccontroller) npccontroller.Save();
        StateController stateController = FindAnyObjectByType<StateController>();
        if (stateController) stateController.Save();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(toSceneIndex);
        FindAnyObjectByType<HeroController>().transform.position = toPosition;
    }
}
