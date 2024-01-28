using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject animator;
    DirectoryInfo dataDir = new DirectoryInfo(Application.dataPath + "\\Data\\");
    public void LoadGame()
    {
        print("Load Game");
        StartCoroutine(PrivateLoadGame());
    }

    public void NewGame()
    {
        print("New Game");
        foreach (FileInfo file in dataDir.GetFiles())
            file.Delete();
        StartCoroutine(PrivateLoadGame());
    }

    IEnumerator PrivateLoadGame()
    {
        animator.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
        print("Quit Game");
        Application.Quit();
    }
}
