using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemInformation : MonoBehaviour
{
    public GameObject ChestItemPrefab;
    private GameObject Panel;
    private void Start()
    {
        Panel = GameManager.instance.ItemInfo;
    }

    private void Update()
    {
        if(Time.timeScale == 1f)
        {
            Panel.SetActive(false);
        }
        else if (Time.timeScale == 0f)
        {
            Panel.SetActive(true);
        }
    }

    public void ItemShowInfo()
    {
        foreach (Transform item in Panel.transform)
        {
            Destroy(item.gameObject);
        }

        

        GameObject obj = Instantiate(ChestItemPrefab, Panel.transform.position, Quaternion.identity, Panel.transform);
        var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
        var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        var itemStacks = obj.transform.Find("ItemStacks").GetComponent<TMP_Text>();
        var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TMP_Text>();

        itemName.text = gameObject.transform.Find("ItemName").GetComponent<TMP_Text>().text;
        itemIcon.sprite = gameObject.transform.Find("ItemIcon").GetComponent<Image>().sprite;
        itemStacks.text = "Current Stacks: " + gameObject.transform.Find("ItemStacks").GetComponent<TMP_Text>().text;
        itemDescription.text = gameObject.transform.Find("ItemDescription").GetComponent<TMP_Text>().text;
    }
}
