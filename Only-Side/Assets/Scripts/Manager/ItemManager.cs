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

    private int previousItemNumberListLength;
    private float totalItemWeight = 0;
    private string selectedItemNumber;
    private List<GameObject> spawnedPrefabSlotList = new List<GameObject>();     // スロットのプレハブオブジェクトを格納するリスト
    private bool isDisplayItemInventory;     // インベントリが見えているか

    //現在の持っているアイテムの合計と持とうとしているアイテム
    public bool CanPickUpItem(float _pickedUpItemWeight)
    {
        totalItemWeight = 0;
        for (int i = 0; i < itemNumberList.Count; i++)
        {
            totalItemWeight += itemDataBase.itemDatas[itemNumberList[i]].weight;
        }
        if (totalItemWeight + _pickedUpItemWeight > PlayerStatus.playerItemWeightLimit)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

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
        // 初期状態でのリストの長さを保存
        previousItemNumberListLength = itemNumberList.Count;
    }

    private void Update()
    {
        SetSlotsIcon();
        SetItemInformation();
        SortItemList();
    }

    // アイテムを拾ったときのアイテムリストへの追加処理
    public void AddItemList(int _id)
    {
        // アイテムリストから引数のIDを探す
        ITEM findItem = itemsList.Find(item => item.id == _id);
        // IDが見つかったら
        if(findItem != null)
        {
            // 個数を増やす
            findItem.count++;
        }
        // 見つからなかった場合
        else
        {
            // 引数のデータを用意する
            ITEM _newItem = new(_id, 1);
            // 用意したデータをリストに追加する
            itemsList.Add(_newItem);
        }
    }

    // アイテム落としたときのアイテムリストへの削除処理
    public void RemoveItemList(int _id)
    {
        // アイテムリストから引数のIDを探す
        ITEM itemToRemove = itemsList.Find(item => item.id == _id);
        // IDが見つかったら
        if (itemToRemove != null)
        {
            // 個数を1減らす
            itemToRemove.count--;
            // 個数が0個になったら
            if (itemToRemove.count <= 0)
            {
                // アイテムリストから削除する
                itemsList.Remove(itemToRemove);
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
                // スロットのプレハブ1を6個を生成
                GameObject _slotPrefabObject = Instantiate(slotPrefabObject, transform.position,
                    Quaternion.identity, slotsObject.transform);
                // リストに入れる
                spawnedPrefabSlotList.Add(_slotPrefabObject);
            }
        }
        if (itemNumberList.Count > previousItemNumberListLength)
        {
            // 仮リストを作る
            List<int> _itemNumberList = new List<int>();
            // 仮リストに要素を入れる
            _itemNumberList.AddRange(itemNumberList);
            // リストを整列させる
            _itemNumberList.Sort();
            // itemNumberListの内容をすべて削除
            itemNumberList.Clear();
            // リストに要素を追加する
            itemNumberList.AddRange(_itemNumberList);

            // itemNumberListの個数かspawnedPrefabSlotListの個数のを比較して最小値をとる
            int _count = Mathf.Min(itemNumberList.Count, spawnedPrefabSlotList.Count);
            for (int i = 0; i < _count; i++)
            {
                // Slotのコンポーネントを取得
                Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                // Slotのコンポーネントを取得できているか
                if (slot != null)
                {
                    // リストの画像をアイコンの画像に
                    slot.itemIconObject.sprite = itemDataBase.itemDatas[itemNumberList[i]].sprite;
                    slot.itemID = itemNumberList[i].ToString();
                }
                // 現在のリストの長さを保存
                previousItemNumberListLength = itemNumberList.Count;
            }
        }
        if (itemNumberList.Count < previousItemNumberListLength)
        {
            for (int i = previousItemNumberListLength; i > itemNumberList.Count - 1; i--)
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
                previousItemNumberListLength = itemNumberList.Count;
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
