using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(MyUpdate());
    }

    IEnumerator MyUpdate()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
        HeroController x = FindAnyObjectByType<HeroController>();
        x.transform.position = new Vector3(10, -2, 0);
    }
}
