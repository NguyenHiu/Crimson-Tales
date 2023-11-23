using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSmileAttackZoneController : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HeroController hero = other.GetComponentInParent<HeroController>();
            hero.TakeDamage(damage);
        }
    }
}
