using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item item1, item2, item3;

    public void AddNewItem() {
        int x = Random.Range(0, 3);
        if (x == 0) {
            inventoryManager.AddItem(item1);
        } else if (x == 1) {
            inventoryManager.AddItem(item2);
        } else {
            inventoryManager.AddItem(item3);
        }
    }
}
