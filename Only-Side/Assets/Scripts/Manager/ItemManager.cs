using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string name;
        public string description;
        public Sprite sprite;
        public float weight;
    }

    public static ItemManager instance;
    public ItemData[] itemData;
    public GameObject itemInventory;

    private bool isDisplayItemInventory;

    private void Start()
    {
        TextAsset _textAsset = new TextAsset();
        _textAsset = Resources.Load("CSV/item", typeof(TextAsset)) as TextAsset;
        itemData = CSVSerializer.Deserialize<ItemData>(_textAsset.text);
        // instance化する
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
        itemInventory.SetActive(isDisplayItemInventory);
    }
}
