using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Sprite closedChestSprite;
    [SerializeField] Sprite openChestSprite;
    SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (Item item in items)
            AddItem(item);
    }

    public void OpenChestSpriteUpdate()
    {
        spriteRenderer.sprite = openChestSprite;
    }

    public void CloseChestSpriteUpdate()
    {
        spriteRenderer.sprite = closedChestSprite;
    }

    void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem childItem = slot.GetComponentInChildren<InventoryItem>();
            if (childItem == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newInventoryItemObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem newInventoryItem = newInventoryItemObject.GetComponent<InventoryItem>();
        newInventoryItem.Init(item);
    }
}