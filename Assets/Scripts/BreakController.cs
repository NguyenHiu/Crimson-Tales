using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    [SerializeField] GameObject boom;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            SwordController sword = other.GetComponentInParent<SwordController>();
            print("sword.name: " + sword.SwordName);
            if (sword.SwordName == "Vo_Phong_Kiem")
            {
                boom.SetActive(true);
                audioManager.Explosion();
            }
        }
    }
}
