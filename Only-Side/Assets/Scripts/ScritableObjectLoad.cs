using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScritableObjectLoad : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
            if (str.IndexOf("/item.csv") != -1)
            {
                Debug.Log("CSVファイルがあった!!!");
                //　Asset直下から読み込む（Resourcesではないので注意）
                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
                string assetfile = str.Replace(".csv", ".asset");
                ItemDataBase _itemDataBase = AssetDatabase.LoadAssetAtPath<ItemDataBase>(assetfile);
                if (_itemDataBase == null)
                {
                    _itemDataBase = new ItemDataBase();
                    AssetDatabase.CreateAsset(_itemDataBase, assetfile);
                }

                _itemDataBase.itemDatas = CSVSerializer.Deserialize<ItemData>(textasset.text);
                EditorUtility.SetDirty(_itemDataBase);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
