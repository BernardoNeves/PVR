using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{

    [Header("Info")]
    public Sprite itemIcon;
    public string itemName;
    public string itemDescription;
    public int itemDropChance;
    public int itemStacks;
}