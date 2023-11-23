using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class EscLayerManager : MonoBehaviour
{
    [SerializeField] GameObject EscapeLayer;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] RequestController requestController;
    [SerializeField] HeroController heroController;
    [SerializeField] Animator animator;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeLayer.SetActive(true);
            heroController.SetDialogOn();
        }
    }

    public void Cancel()
    {
        EscapeLayer.SetActive(false);
        heroController.SetDialogOff();
    }

    public void QuitGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            NPCController npccontroller = FindAnyObjectByType<NPCController>();
            if (npccontroller) npccontroller.Save();
            StateController stateController = FindAnyObjectByType<StateController>();
            if (stateController) stateController.Save();

            string data = "";
            foreach (InventorySlot inventorySlot in inventoryManager.inventorySlots)
            {
                if (inventorySlot.transform.childCount == 0)
                    data += ",";
                else data += inventorySlot.
                             GetComponentInChildren<InventoryItem>().
                             ItemName + ",";
            }
            data += SceneManager.GetActiveScene().buildIndex.ToString() + ",";
            data += heroController.transform.position.x.ToString() + "," +
                heroController.transform.position.y.ToString() + "," +
                heroController.Health.ToString() + "|";
            foreach (GameObject requestManagerOB in requestController.requestManagers)
            {
                RequestInfo requestInfo = requestManagerOB.GetComponent<RequestManager>().requestInfo;
                data += requestInfo.name + "," + requestInfo.demand.ToString() + "," + requestInfo.sampleItem.ItemName;
            }
            SAVE.SaveFile(data, "Data\\gameData");
        }
        StartCoroutine(Animation());
        // Application.Quit();
    }

    public void PlayerDeath()
    {
        DirectoryInfo dataDir = new(Application.dataPath + "\\Data\\");
        foreach (FileInfo file in dataDir.GetFiles())
            file.Delete();
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        DestroyAll();
        SceneManager.LoadScene(0);
    }

    void DestroyAll()
    {
        DontDestroyOnLoad[] gos = Resources.FindObjectsOfTypeAll<DontDestroyOnLoad>();
        foreach (DontDestroyOnLoad go in gos)
        {
            Destroy(go.gameObject);
        }
    }
}
