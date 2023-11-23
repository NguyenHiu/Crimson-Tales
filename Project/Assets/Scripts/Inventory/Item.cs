using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;
    [SerializeField] string description;
    [SerializeField] ItemType itemType;
    [SerializeField] int damage;
    [SerializeField] PotionType potionType;

    public int Damage { get { return damage; } }
    public string ItemName { get { return itemName; } }
    public PotionType GetPotionType { get { return potionType; } }
    public ItemType GetItemType { get { return itemType; } }
    public Sprite Sprite { get { return sprite; } }
}

public enum ItemType
{
    Potion,
    Sword,
    Herb
}

public enum PotionType
{
    Health,
    Speed
}