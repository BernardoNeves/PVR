using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface InteractableInterface
{
    public void Interact();

}

public class chestOpen : MonoBehaviour, InteractableInterface
{
    public List<ItemData> Drops = new List<ItemData>();
    public GameObject ChestItemPrefab;


    List<ItemData> GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<ItemData> DroppedItems = new List<ItemData>();

        List<ItemData> possibleItems = new List<ItemData>();
        foreach (ItemData item in Drops)
        {
            if (randomNumber <= item.itemDropChance)
            {
                possibleItems.Add(item);
            }
        }
        while (possibleItems.Count > 0 && DroppedItems.Count < 3)
        {
            ItemData droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            possibleItems.Remove(droppedItem);
            DroppedItems.Add(droppedItem);
        }
        return DroppedItems;
    }

    public void Interact()
    {
        List<ItemData> CurrentChestItems = GetDroppedItem();
        if(CurrentChestItems.Count == 0)
        {
            GameManager.instance.PlayerHealth.Damage(25 * Random.Range(1, 5));
            Destroy(gameObject);

            return;
        }

        GameManager.instance.CursorToggle(true);
        GameManager.instance.ChestUI.SetActive(true);
        ListItems(CurrentChestItems);

        Destroy(gameObject);
    }

    public void ListItems(List<ItemData> Items)
    {
        foreach (Transform item in GameManager.instance.ChestContent)
        {
            Destroy(item.gameObject);
        }

        foreach (ItemData item in Items)
        {
            GameObject obj = Instantiate(ChestItemPrefab, GameManager.instance.ChestContent);
            obj.GetComponent<ItemPick>().itemData = item;

            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemStacks = obj.transform.Find("ItemStacks").GetComponent<TMP_Text>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();


            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;
            itemStacks.text = "Current Stacks: " + item.itemStacks.ToString();
            itemDescription.text = item.itemDescription;
        }
    }
}

