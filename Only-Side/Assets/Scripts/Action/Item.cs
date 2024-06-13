using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemDataBase itemDataBase;
    public int itemNumber;

    private void Start()
    {
        GetComponent<Image>().sprite = itemDataBase.itemDatas[itemNumber].sprite;
    }

    public void PickupItem()
    {
        /*
        if (ItemManager.instance.CanPickUpItem())
        {
            ItemManager.instance.itemNumberList.Add(itemNumber);
            Destroy(gameObject);
        }*/
        ItemManager.instance.itemNumberList.Add(itemNumber);
        Destroy(gameObject);
    }
}
