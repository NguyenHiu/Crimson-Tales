using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTriggerTutorial : MonoBehaviour
{
    [SerializeField] Tutorial tutorial;
    bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (isTriggered) return;
        if (collider2D.CompareTag("PlayerHitbox"))
        {
            tutorial.FromMoveToNPC();
            isTriggered = true;
        }
    }
}
