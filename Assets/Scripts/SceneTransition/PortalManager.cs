using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public int toSceneIndex;
    public Vector2 toPosition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(toSceneIndex);
            FindAnyObjectByType<HeroController>().transform.position = toPosition;
        }
    }
}
