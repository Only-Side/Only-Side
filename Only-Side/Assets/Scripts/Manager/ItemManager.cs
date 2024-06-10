using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public GameObject itemInventoryObject;
    public InventoryUI inventoryUI;
    public List<int> itemNumberList;
    public List<GameObject> spawnedPrefabSlotList = new List<GameObject>();
    public GameObject slotPrefabObject;
    public GameObject slotsObject;
    public ItemDataBase itemDataBase;

    private bool isDisplayItemInventory;

    private void Start()
    {
        // シングルトン化
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        SetSlots();
    }

    public void OnInventoryMenu(InputAction.CallbackContext context)
    {
        if(isDisplayItemInventory)
        {
            isDisplayItemInventory = false;
        }
        else
        {
            isDisplayItemInventory = true;
        }
        itemInventoryObject.SetActive(isDisplayItemInventory);
    }

    private void SetSlots()
    {
        while (spawnedPrefabSlotList.Count < 16)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject _slotPrefabObject = Instantiate(slotPrefabObject, transform.position,
                    Quaternion.identity, slotsObject.transform);
                spawnedPrefabSlotList.Add(_slotPrefabObject);
            }
        }
        for (int i = 0;i < spawnedPrefabSlotList.Count; i++)
        {
            Image image = spawnedPrefabSlotList[i].GetComponentInChildren<Image>();
            if(image != null) 
            {
                image.sprite = itemDataBase.itemDatas[itemNumberList[i]].sprite;
            }
        }
    }
}
