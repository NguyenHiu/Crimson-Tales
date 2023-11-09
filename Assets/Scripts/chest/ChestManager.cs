using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public List<Item> items;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    void Start()
    {
        foreach (Item item in items)
            AddItem(item);
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