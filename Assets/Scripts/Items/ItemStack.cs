using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item _item;
    public int _stacks;
    public string _name;
    [HideInInspector]
    public ItemData _itemStackData;

    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public int Stacks
    {
        get
        {
            return _stacks;
        }
        set
        {
            _stacks = value;
        }
    }

    public ItemData ItemStackData
    {
        get
        {
            return _itemStackData;
        }
        set
        {
            _itemStackData = value;
        }
    }


    public ItemStack(Item newItem, int newStacks, ItemData newItemStackData)
    {
        _item = newItem;
        _stacks = newStacks;
        _itemStackData = newItemStackData;
        _name = newItemStackData.itemName;
    }
}
