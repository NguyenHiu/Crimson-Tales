using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class DetectHero : MonoBehaviour
{
    public Collider2D collider2d;
    public GoblinController goblin;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // print("detect hero at: " + goblin.heroPosition);
            goblin.heroPosition = other.GetComponent<HeroController>().rb.position;
            if (goblin.transform.position.y > (goblin.heroPosition.y - 0.28f)) {
                other.GetComponent<HeroController>().SetHighLayerObject();
                goblin.SetLowLayerObject();
                print("hero high, goblin low");
            } else {
                other.GetComponent<HeroController>().SetLowLayerObject();
                goblin.SetHighLayerObject();
                print("goblin high, hero low");
            }
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            goblin.heroPosition = Vector2.zero;
        }
    }
}
