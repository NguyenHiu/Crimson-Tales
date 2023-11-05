using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public String itemName;
    public String description;  
    public ItemType itemType;
    public int damage;
    public PotionType potionType;

    public int GetDamage() {
        return damage;
    }

    public PotionType GetPotionType() {
        return potionType;
    }
}

public enum ItemType {
    Potion,
    Sword
}

public enum PotionType {
    Health,
    Speed
}