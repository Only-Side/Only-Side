using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemNumber;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = ItemManager.itemDataSprite[itemNumber];
    }

    public void PickupItem()
    {
        bool _canPickUpItem = ItemManager.instance.CanPickUpItem(ItemManager.itemDataWeight[itemNumber]);
        if (_canPickUpItem)
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
