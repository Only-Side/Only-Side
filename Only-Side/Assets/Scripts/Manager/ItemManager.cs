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

    public List<int> itemNumberList;     // 取得アイテムを格納するリスト
    public GameObject itemInventoryObject;     // アイテムのインベントリオブジェクト
    public GameObject slotPrefabObject;     // スロットのプレハブ
    public GameObject slotsObject;     // スロットのプレハブの親オブジェクト
    public ItemDataBase itemDataBase;     // アイテムのデータベース

    private List<GameObject> spawnedPrefabSlotList = new List<GameObject>();     // スロットのプレハブオブジェクトを格納するリスト
    private bool isDisplayItemInventory;     // インベントリが見えているか

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
        SetSlotsIcon();
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
        if (itemNumberList.Count > spawnedPrefabSlotList.Count)
        {

        }
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
            }
        }
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
        }
        // インベントリのアクティブを設定
        itemInventoryObject.SetActive(isDisplayItemInventory);
    }
}
