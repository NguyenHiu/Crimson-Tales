using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartsController : MonoBehaviour
{
    public GameObject heartPrefab;
    public HeroController player;
    List<HeartController> hearts = new();

    public void ClearAllHearts()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject)
                Destroy(t.gameObject);
        }
        hearts = new();
    }

    public void DrawHearts()
    {
        ClearAllHearts();
        int noHeartsNeedToDraw = (int)Math.Ceiling(player.MaxHealth / 2.0);

        // create player's max health
        for (int i = 0; i < noHeartsNeedToDraw; i++)
        {
            CreateNewHearts();
        }

        // display player's current health
        for (int i = 0; i < hearts.Count; i++)
        {
            int t = player.Health - 2 * i;
            if (t <= 0)
                hearts[i].SetHeartImage(HeartStatus.Empty);
            else if (t >= 2)
                hearts[i].SetHeartImage(HeartStatus.Full);
            else
                hearts[i].SetHeartImage(HeartStatus.Half);
        }
    }

    public void CreateNewHearts()
    {
        GameObject newObject = Instantiate(heartPrefab);
        newObject.transform.SetParent(transform, false);

        HeartController newHeart = newObject.GetComponent<HeartController>();
        newHeart.SetHeartImage(HeartStatus.Empty);
        hearts.Add(newHeart);
    }

    // Update is called once per frame
    void Update()
    {
        DrawHearts();
    }
}
