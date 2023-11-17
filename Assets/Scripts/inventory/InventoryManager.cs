using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public int selectedSlot = -1;

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();
        selectedSlot = newValue;
        inventorySlots[selectedSlot].Select();
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem childItem = slot.GetComponentInChildren<InventoryItem>();
            if (childItem == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newInventoryItemObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem newInventoryItem = newInventoryItemObject.GetComponent<InventoryItem>();
        newInventoryItem.Init(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInslot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInslot != null)
        {
            Item item = itemInslot.Item;
            if (use == true)
            {
                if (itemInslot.gameObject)
                    Destroy(itemInslot.gameObject);
            }
            return item;
        }
        return null;
    }

    public int NumberOfItem(Item y)
    {
        int cnt = 0;
        for (int i = 0; i < inventorySlots.Length; ++i)
        {
            InventoryItem childItem = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (childItem && childItem.Item == y)
            {
                ++cnt;
            }
        }
        return cnt;
    }
}
