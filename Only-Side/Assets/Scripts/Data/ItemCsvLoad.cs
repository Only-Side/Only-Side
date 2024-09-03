using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemCsvLoad : MonoBehaviour
{
    public TextAsset csvFile; // UnityエディタからCSVファイルをアタッチします
    public ItemDataBase itemDataBase;

    public void LoadData()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' }); // 行ごとに分割

        itemDataBase.itemDatas = new ItemData[data.Length - 1]; // ヘッダー行を除外

        for (int i = 1; i < data.Length; i++) // 最初の行はヘッダーなのでスキップ
        {
            string[] row = data[i].Split(new char[] { ',' }); // 各行をカンマで分割

            ItemData item = new ItemData();
            item.name = row[0];
            item.description = row[1];
            item.sprite = LoadSprite(row[2]); // 画像の読み込みを実装
            item.weight = float.Parse(row[3]);

            itemDataBase.itemDatas[i - 1] = item;
        }

        Sprite LoadSprite(string spriteName)
        {
            // Resourcesフォルダからスプライトを読み込む（スプライト名がCSVに記載されている場合）
            return Resources.Load<Sprite>(spriteName);
        }
    }

    void Start()
    {
        LoadData();
    }
}
