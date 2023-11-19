using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySmallRocks : MonoBehaviour
{
    [SerializeField] GameObject boom;
    [SerializeField] GameObject[] rocks;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
            boom.SetActive(true);
    }

    public void DestroyRocks()
    {
        foreach (GameObject rock in rocks)
            Destroy(rock);
        Destroy(boom);
    }
}
