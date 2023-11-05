using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public ItemType itemType;
    public ActionType actionType;
    public bool stackable = false;


}

public enum ItemType {
    Poision,
    Weapon
}

public enum ActionType {
    Drink,
    Attack
}
