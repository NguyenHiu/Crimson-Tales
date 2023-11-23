using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class DetectHero : MonoBehaviour
{
    public Collider2D collider2d;
    public Enemy enemy;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            Transform playerTransform = other.GetComponentInParent<HeroController>().transform;
            enemy.SetAStarDestination(playerTransform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
            enemy.SetAStarDestination(null);
    }
}
