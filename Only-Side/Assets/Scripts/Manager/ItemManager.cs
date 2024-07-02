using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<ITEM> itemsList;
    public List<int> itemNumberList;     // 取得アイテムを格納するリスト
    public GameObject itemInventoryObject;     // アイテムのインベントリオブジェクト
    public GameObject slotPrefabObject;     // スロットのプレハブ
    public GameObject slotsObject;     // スロットのプレハブの親オブジェクト
    public TextMeshProUGUI itemNameTextObject;
    public TextMeshProUGUI itemDescriptionTextObject;
    public ItemDataBase itemDataBase;     // アイテムのデータベース

    private int previousItemsListLength;
    private float totalItemWeight = 0;
    private string selectedItemNumber;
    private List<GameObject> spawnedPrefabSlotList = new List<GameObject>();     // スロットのプレハブオブジェクトを格納するリスト
    private Dictionary<ITEM, int> previousItemCount = new Dictionary<ITEM, int>();
    private bool isDisplayItemInventory;     // インベントリが見えているか

    //現在の持っているアイテムの合計と持とうとしているアイテム
    //public bool CanPickUpItem(float _pickedUpItemWeight)
    //{
    //    totalItemWeight = 0;
    //    for (int i = 0; i < itemNumberList.Count; i++)
    //    {
    //        totalItemWeight += itemDataBase.itemDatas[itemNumberList[i]].weight;
    //    }
    //    if (totalItemWeight + _pickedUpItemWeight > PlayerStatus.playerItemWeightLimit)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    private void Awake()
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

    private void Start()
    {
        previousItemsListLength = itemsList.Count;
        foreach (var item in itemsList)
        {
            if (!previousItemCount.ContainsKey(item))
            {
                previousItemCount[item] = item.count;
            }
        }
    }

    private void Update()
    {
        SetSlotsIcon();
        SetItemInformation();
        foreach (var item in itemsList)
        {
            if (!previousItemCount.ContainsKey(item))
            {
                previousItemCount[item] = item.count;
            }

            if (previousItemCount[item] != item.count)
            {
                int _count = Mathf.Min(itemsList.Count, spawnedPrefabSlotList.Count);
                for (int i = 0; i < _count; i++)
                {
                    Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                    if (slot != null)
                    {
                        slot.itemCount.text = itemsList[i].count.ToString();
                    }
                }
                previousItemCount[item] = item.count;
            }
        }
    }

    public void AddItemList(int _id)
    {
        ITEM addItem = itemsList.Find(item => item.id == _id);
        if (addItem != null)
        {
            addItem.count++;
            if (!previousItemCount.ContainsKey(addItem))
            {
                previousItemCount[addItem] = addItem.count;
            }
        }
        else
        {
            ITEM _newItem = new(_id, 1);
            itemsList.Add(_newItem);
            if (!previousItemCount.ContainsKey(_newItem))
            {
                previousItemCount[_newItem] = _newItem.count;
            }
        }
    }

    public void RemoveItemList(int _id)
    {
        ITEM itemToRemove = itemsList.Find(item => item.id == _id);
        if (itemToRemove != null)
        {
            itemToRemove.count--;
            if (itemToRemove.count <= 0)
            {
                itemsList.Remove(itemToRemove);
                previousItemCount.Remove(itemToRemove);
            }
        }
    }


    // スロットのアイコンを設定
    private void SetSlotsIcon()
    {
        while (spawnedPrefabSlotList.Count < 16)
        {
            for (int i = 0; i < 16; i++)
            {
                // スロットのプレハブを16個を生成
                GameObject _slotPrefabObject = Instantiate(slotPrefabObject, transform.position,
                    Quaternion.identity, slotsObject.transform);
                // リストに入れる
                spawnedPrefabSlotList.Add(_slotPrefabObject);
            }
        }
        if (itemsList.Count > previousItemsListLength)
        {
            // itemNumberListの個数かspawnedPrefabSlotListの個数のを比較して最小値をとる
            int _count = Mathf.Min(itemsList.Count, spawnedPrefabSlotList.Count);
            for (int i = 0; i < _count; i++)
            {
                SortItemList();
                // Slotのコンポーネントを取得
                Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                // Slotのコンポーネントを取得できているか
                if (slot != null)
                {
                    // リストの画像をアイコンの画像に
                    slot.itemIconObject.sprite = itemDataBase.itemDatas[itemsList[i].id].sprite;
                    slot.itemID = itemsList[i].id.ToString();
                    slot.itemCount.text = itemsList[i].count.ToString();
                }
                // 現在のリストの長さを保存
                previousItemsListLength = itemsList.Count;
            }
        }
        if (itemsList.Count < previousItemsListLength)
        {
            for (int i = previousItemsListLength; i > itemsList.Count - 1; i--)
            {
                // Slotのコンポーネントを取得
                Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                // Slotのコンポーネントを取得できているか
                if (slot != null)
                {
                    // リストの画像をアイコンの画像に
                    slot.itemIconObject.sprite = null;
                }
                // 現在のリストの長さを保存
                previousItemsListLength = itemsList.Count;
            }
        }
    }

    private void SetItemInformation()
    {
        // インベントリが見えていたら
        if(isDisplayItemInventory) 
        {
            // 選択されているオブジェクトがあるか
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                // 選択されているオブジェクトのSlotのコンポーネントを取得
                Slot slot = EventSystem.current.currentSelectedGameObject.GetComponent<Slot>();
                // コンポーネントを取得できているか
                if (slot != null)
                {
                    // 選択されているアイテムの番号を取得
                    selectedItemNumber = slot.itemID;
                }
            }
            // 選択されているアイテム番号が取得できているか
            if(selectedItemNumber != "")
            {
                // int型へ変換
                int _selectedItemNumber = int.Parse(selectedItemNumber);
                // アイテム詳細の名前のテキストをアイテム名に設定
                itemNameTextObject.text = itemDataBase.itemDatas[_selectedItemNumber].name;
                // アイテム情報の詳細情報のテキストをアイテム詳細情報に設定
                itemDescriptionTextObject.text =
                    itemDataBase.itemDatas[_selectedItemNumber].description;
            }
        }
    }

    // ItemListを整列させる
    private void SortItemList()
    {
        // IDの昇順で整列
        itemsList.Sort((x, y) => x.id - y.id);
    }

    // InputActionのInventoryMenuが押されたとき実行
    public void OnInventoryMenu(InputAction.CallbackContext context)
    {
        // インベントリが見えていたら
        if (isDisplayItemInventory)
        {
            // オフに
            isDisplayItemInventory = false;
        }
        // インベントリが見えていないなら
        else
        {
            // オンに
            isDisplayItemInventory = true;
            // 初期選択位置を設定
            EventSystem.current.firstSelectedGameObject = spawnedPrefabSlotList[0];
            EventSystem.current.SetSelectedGameObject(spawnedPrefabSlotList[0]);
        }
        // インベントリのアクティブを設定
        itemInventoryObject.SetActive(isDisplayItemInventory);
    }
}

[System.Serializable]
public class ITEM
{
    public int id;
    public int count;

    public ITEM(int id, int count)
    {
        this.id = id;
        this.count = count;
    }
}
