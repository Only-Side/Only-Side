//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class ScriptLoad : AssetPostprocessor
//{
//    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//    {
//        foreach (string str in importedAssets)
//        {
//            //　IndexOfの引数は"/(読み込ませたいファイル名)"とする。
//            if (str.IndexOf("/script.csv") != -1)
//            {
//                Debug.Log("script.csvファイルがあった!!!");
//                //　Asset直下から読み込む（Resourcesではないので注意）
//                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
//                //　同名のScriptableObjectファイルを読み込む。ない場合は新たに作る。
//                string assetfile = str.Replace(".csv", ".asset");
//                ScriptDataBase scriptDataBase = AssetDatabase.LoadAssetAtPath<ScriptDataBase>(assetfile);
//                if (scriptDataBase == null)
//                {
//                    scriptDataBase = new ScriptDataBase();
//                    AssetDatabase.CreateAsset(scriptDataBase, assetfile);
//                }

//                scriptDataBase.scriptDatas = CSVSerializer.Deserialize<ScriptData>(textasset.text);
//                EditorUtility.SetDirty(scriptDataBase);
//                AssetDatabase.SaveAssets();
//            }
//        }
//    }
//}
