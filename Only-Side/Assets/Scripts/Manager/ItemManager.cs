using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public GameObject itemInventoryObject;
    public InventoryUI inventoryUI;
    public List<int> itemNumberList;

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
}
