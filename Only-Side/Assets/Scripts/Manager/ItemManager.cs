using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    #region シングルトン化

    public static ItemManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        // アイテムデータを読み込む
        LoadItemData();
    }

    #endregion シングルトン化

    public static string[] itemDataName;
    public static string[] itemDataDescription;
    public static Sprite[] itemDataSprite;
    public static float[] itemDataWeight;

    public List<ITEM> itemList;
    public GameObject itemInventoryObject;     // アイテムのインベントリオブジェクト
    public GameObject slotPrefabObject;     // スロットのプレハブ
    public GameObject slotsObject;     // スロットのプレハブの親オブジェクト
    public TextMeshProUGUI itemNameTextObject;     // アイテムの名前
    public TextMeshProUGUI itemDescriptionTextObject;     // アイテムの詳細情報
    public ItemDataBase itemDataBase;     // アイテムのデータベース

    private int previousItemListLength;
    private int selectedItemNumber;
    private int itemSlotQuantity = 4;     // 表示するスロットの数
    private float totalItemWeight = 0;
    private List<GameObject> spawnedPrefabSlotList = new List<GameObject>();     // スロットのプレハブオブジェクトを格納するリスト
    private Dictionary<ITEM, int> previousItemCount = new Dictionary<ITEM, int>();
    private bool isDisplayItemInventory;     // インベントリが見えているか

    //現在の持っているアイテムの合計と持とうとしているアイテム
    public bool CanPickUpItem(float pickedUpItemWeight)
    {
        totalItemWeight = 0;
        for(int i = 0; i < itemList.Count; i++)
        {
            totalItemWeight += itemDataWeight[i] * itemList[i].count;
        }
        if(totalItemWeight + pickedUpItemWeight > PlayerStatus.playerItemWeightLimit)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Start()
    {
        // 初期状態でのリストの長さを保存
        previousItemListLength = itemList.Count;
        // 各アイテムのカウントをディクショナリに保存
        foreach (var item in itemList)
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
        MonitorItemListCount();
    }

    // ScriptableObjectからデータを読み込む
    private void LoadItemData()
    {
        if (itemDataBase == null)
        {
            print("ItemDataBase is not found!");
        } 
        else
        {
            int length = itemDataBase.itemDatas.Length;
            itemDataName = new string[length];
            itemDataDescription = new string[length];
            itemDataSprite = new Sprite[length];
            itemDataWeight = new float[length];
            for (int i = 0; i < length; i++)
            {
                itemDataName[i] = itemDataBase.itemDatas[i].name;
                itemDataDescription[i] = itemDataBase.itemDatas[i].description;
                itemDataSprite[i] = itemDataBase.itemDatas[i].sprite;
                itemDataWeight[i] = itemDataBase.itemDatas[i].weight;
            }
        }
    }

    public void AddItemList(int _id)
    {
        // アイテムリストから引数のIDを探す
        ITEM findItem = itemList.Find(item => item.id == _id);
        if (findItem != null)
        {
            // 個数を増やす
            findItem.count++;
            // ディクショナリにキーが存在しない場合は追加
            if (!previousItemCount.ContainsKey(findItem))
            {
                previousItemCount[findItem] = findItem.count;
            }
        }
        else
        {
            // 新しいアイテムを追加
            ITEM _newItem = new(_id, 1);
            itemList.Add(_newItem);
            // 新しいアイテムをディクショナリに追加
            if (!previousItemCount.ContainsKey(_newItem))
            {
                previousItemCount[_newItem] = _newItem.count;
            }
        }
    }

    public void RemoveItemList(int _id)
    {
        // アイテムリストから引数のIDを探す
        ITEM itemToRemove = itemList.Find(item => item.id == _id);
        if (itemToRemove != null)
        {
            // 個数を1減らす
            itemToRemove.count--;
            // 個数が0個になったらアイテムリストから削除する
            if (itemToRemove.count <= 0)
            {
                itemList.Remove(itemToRemove);
                // ディクショナリからも削除する
                previousItemCount.Remove(itemToRemove);
            }
        }
    }

    private void MonitorItemListCount()
    {
        foreach (var item in itemList)
        {
            // ディクショナリにキーが存在しない場合は追加
            if (!previousItemCount.ContainsKey(item))
            {
                previousItemCount[item] = item.count;
            }

            // アイテムのカウントが変わった場合
            if (previousItemCount[item] != item.count)
            {
                int _count = Mathf.Min(itemList.Count, spawnedPrefabSlotList.Count);
                for (int i = 0; i < _count; i++)
                {
                    Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                    if (slot != null)
                    {
                        slot.itemCount.text = itemList[i].count.ToString();
                    }
                }
                // ディクショナリを更新
                previousItemCount[item] = item.count;
            }
        }
    }

    // スロットのアイコンを設定
    private void SetSlotsIcon()
    {
        while (spawnedPrefabSlotList.Count < itemSlotQuantity)
        {
            for (int i = 0; i < itemSlotQuantity; i++)
            {
                // スロットのプレハブを16個を生成
                GameObject _slotPrefabObject = Instantiate(slotPrefabObject, transform.position,
                    Quaternion.identity, slotsObject.transform);
                // リストに入れる
                spawnedPrefabSlotList.Add(_slotPrefabObject);
            }
        }
        if (itemList.Count > previousItemListLength)
        {
            // itemNumberListの個数かspawnedPrefabSlotListの個数のを比較して最小値をとる
            int _count = Mathf.Min(itemList.Count, spawnedPrefabSlotList.Count);
            for (int i = 0; i < _count; i++)
            {
                SortItemList();
                // Slotのコンポーネントを取得
                Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                // Slotのコンポーネントを取得できているか
                if (slot != null)
                {
                    slot.itemIconObject.color = Color.white;
                    // リストの画像をアイコンの画像に
                    slot.itemIconObject.sprite = itemDataSprite[itemList[i].id];
                    slot.itemID = itemList[i].id;
                    slot.itemCount.text = itemList[i].count.ToString();
                }
                // 現在のリストの長さを保存
                previousItemListLength = itemList.Count;
            }
        }
        if (itemList.Count < previousItemListLength)
        {
            for (int i = previousItemListLength; i > itemList.Count - 1; i--)
            {
                // Slotのコンポーネントを取得
                Slot slot = spawnedPrefabSlotList[i].GetComponent<Slot>();
                // Slotのコンポーネントを取得できているか
                if (slot != null)
                {
                    slot.itemIconObject.color = Color.clear;
                    // リストの画像をアイコンの画像に
                    slot.itemIconObject.sprite = null;
                }
                // 現在のリストの長さを保存
                previousItemListLength = itemList.Count;
            }
        }
        for(int i = 0; i < itemList.Count; i++)
        {
            if(itemList[i].count <= 0)
            {
                RemoveItemList(itemList[i].id);
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
            if(selectedItemNumber != -1)
            {
                // アイテム詳細の名前のテキストをアイテム名に設定
                itemNameTextObject.text = itemDataName[selectedItemNumber];
                // アイテム情報の詳細情報のテキストをアイテム詳細情報に設定
                itemDescriptionTextObject.text =
                    itemDataDescription[selectedItemNumber];
            }
        }
    }

    // ItemListを整列させる
    private void SortItemList()
    {
        // IDの昇順で整列
        itemList.Sort((x, y) => x.id - y.id);
    }

    // InputActionのInventoryMenuが押されたとき実行
    public void OnInventoryMenu(InputAction.CallbackContext context)
    {
        // インベントリが見えていたら
        if (isDisplayItemInventory)
        {
            // オフに
            isDisplayItemInventory = false;
            // プレイヤーのコントロールをできなくする
            PlayerControl.isPlayerControl = true;
        }
        // インベントリが見えていないなら
        else
        {
            // オンに
            isDisplayItemInventory = true;
            // プレイヤーのコントロールをできなくする
            PlayerControl.isPlayerControl = false;
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
