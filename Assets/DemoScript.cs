using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item item;

    public void AddNewItem() {
        inventoryManager.AddItem(item);
    }
}
