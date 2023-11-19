using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public int toSceneIndex;
    public Vector2 toPosition;
    [SerializeField] MyTransition myTransition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NPCController npccontroller = FindAnyObjectByType<NPCController>();
            if (npccontroller) npccontroller.Save();
            StateController stateController = FindAnyObjectByType<StateController>();
            if (stateController) stateController.Save();
            StartCoroutine(MyLoadScene());
        }
    }

    IEnumerator MyLoadScene()
    {
        myTransition.ActiveEndTransition();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(toSceneIndex);
        FindAnyObjectByType<HeroController>().transform.position = toPosition;
    }
}
