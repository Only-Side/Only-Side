using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemNumber;

    private void Start()
    {
        GetComponent<Image>().sprite = ItemManager.itemDataSprite[itemNumber];
    }

    public void PickupItem()
    {
        if (ItemManager.instance.CanPickUpItem(
            ItemManager.itemDataWeight[itemNumber]))
        {
            ItemManager.instance.AddItemList(itemNumber);
            Destroy(gameObject);
        }
    }

    public void DropItem()
    {
        ItemManager.instance.RemoveItemList(itemNumber);
        Destroy(gameObject);
    }
}
