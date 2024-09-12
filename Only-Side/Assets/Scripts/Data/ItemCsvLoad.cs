using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCsvLoad : MonoBehaviour
{
    public TextAsset csvFile; // UnityエディタからCSVファイルをアタッチします
    public ItemDataBase itemDataBase;

    public void LoadData()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries); // 行ごとに分割し、空の行を無視

        itemDataBase.itemDatas = new ItemData[data.Length - 1]; // ヘッダー行を除外

        for (int i = 1; i < data.Length; i++) // 最初の行はヘッダーなのでスキップ
        {
            string[] row = data[i].Split(new char[] { ',' }); // 各行をカンマで分割

            if (row.Length < 4)
            {
                Debug.LogWarning($"CSV行のデータが不足しています: {data[i]}");
                continue;
            }

            ItemData item = new ItemData();
            item.name = row[0];
            item.description = row[1];
            item.sprite = LoadSprite(row[2]); // スプライトの読み込み（未実装でも問題なし）
            item.weight = float.TryParse(row[3], out float weight) ? weight : 0f;

            itemDataBase.itemDatas[i - 1] = item;
        }
    }

    Sprite LoadSprite(string spriteName)
    {
        // スプライトの読み込み（スプライト名がCSVに記載されている場合）
        // スプライトの読み込みを実装する場合は以下のコードをアンコメントして使用
        // Sprite sprite = Resources.Load<Sprite>(spriteName);

        // スプライトが未実装の場合はnullを返す
        return null;
    }

    void Start()
    {
        LoadData();
    }
}
