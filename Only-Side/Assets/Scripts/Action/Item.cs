using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int itemNumber;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = ItemManager.itemDataSprite[itemNumber];
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
