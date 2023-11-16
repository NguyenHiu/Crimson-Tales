using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Item item;
    SpriteRenderer spriteRenderer;
    HeroController player = null;

    void Update()
    {
        if (player != null)
            SendItem();
    }

    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (item)
            spriteRenderer.sprite = item.sprite;
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetItemDropped(Item _item)
    {
        item = _item;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            player = collider2D.GetComponent<HeroController>();
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
            player = null;
    }

    void SendItem()
    {
        if (player.ReceiveItem(item))
        {
            player = null;
            if (gameObject)
                Destroy(gameObject);
        }
    }
}