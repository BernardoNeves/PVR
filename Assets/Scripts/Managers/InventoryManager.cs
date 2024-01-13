using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> Items = new List<ItemData>();

    public Transform ItemContent;
    public GameObject InventoryItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(ItemData item)
    {
        Items.Add(item);
    }

    public void Remove(ItemData item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (ItemData item in Items)
        {
            GameObject obj = Instantiate(InventoryItemPrefab, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemStacks = obj.transform.Find("ItemStacks").GetComponent<TMP_Text>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();


            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;
            itemStacks.text = item.itemStacks.ToString();
            itemDescription.text = item.itemDescription;
        }
    }
}
