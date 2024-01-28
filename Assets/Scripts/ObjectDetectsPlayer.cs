using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectsPlayer : MonoBehaviour
{
    [SerializeField] ButtonHint btnHint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            btnHint.ShowButton();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            btnHint.HideButton();
        }
    }
}
