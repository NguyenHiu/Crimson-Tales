using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Item item;
    SpriteRenderer spriteRenderer;

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
        {
            print("return item");
            collider2D.GetComponent<HeroController>().ReceiveItem(item);
            Destroy(gameObject);
            return;
        }
    }
}
