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
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = other.GetComponent<HeroController>().transform;
            enemy.SetAStarDestination(playerTransform);
            if (enemy.transform.position.y > (playerTransform.position.y - 0.28f))
            {
                other.GetComponent<HeroController>().SetHighLayerObject();
                enemy.SetLowLayerObject();
            }
            else
            {
                other.GetComponent<HeroController>().SetLowLayerObject();
                enemy.SetHighLayerObject();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            enemy.SetAStarDestination(null);
    }
}
