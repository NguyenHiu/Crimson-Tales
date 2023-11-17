using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene(1);
        HeroController x = FindAnyObjectByType<HeroController>();
        x.transform.position = new Vector3(10, -2, 0);
    }
}
