using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // アイテムを格納する構造体
    public struct ITEM
    {
        public string itemName;
        public string itemInformation;
        public string itemImage;
        public float itemWeight;
    }
    public static List<ITEM> itemList = new List<ITEM>();     // アイテムの構造体を格納するリスト
    public GameObject itemInventory;

    // ITEM構造体のcsvファイルを読み込む
    public List<ITEM> ITEM_read_csv()
    {
        // 一時入力用で毎回初期化する構造体とリスト
        ITEM item = new ITEM();
        List<ITEM> item_list = new List<ITEM>();

        // ResourcesからCSVを読み込むのに必要
        TextAsset itemCsvFile;

        // 読み込んだCSVファイルを格納
        List<string[]> itemCsvDatas = new List<string[]>();

        // CSVファイルの行数を格納
        int height = 0;

        // for文用。一行目は読み込まない
        int i = 1;

        /* Resouces/CSV下のCSV読み込み */
        itemCsvFile = Resources.Load("CSV/enemy") as TextAsset;
        // 読み込んだテキストをString型にして格納
        StringReader reader = new StringReader(itemCsvFile.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // ,で区切ってCSVに格納
            itemCsvDatas.Add(line.Split(','));
            height++; // 行数加算
        }
        for (i = 1; i < height; i++)
        {
            // csvDatasはString型なのでそのまま格納できる
            item.itemName = itemCsvDatas[i][0];
            item.itemInformation = itemCsvDatas[i][1];
            item.itemImage = itemCsvDatas[i][2];
            // String型をfloat型にして格納
            item.itemWeight = Convert.ToSingle(itemCsvDatas[i][3]);

            // 戻り値のリストに加える
            item_list.Add(item);
        }
        return item_list;
    }

    private void Start()
    {
        // アイテムリストに読み込んだ情報を反映
        itemList = ITEM_read_csv();
    }

    private void Update()
    {
        
    }
    
    //private void OnInventoryMenu(Input
}
