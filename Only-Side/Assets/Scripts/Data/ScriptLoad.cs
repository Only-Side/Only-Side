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
//            //�@IndexOf�̈�����"/(�ǂݍ��܂������t�@�C����)"�Ƃ���B
//            if (str.IndexOf("/script.csv") != -1)
//            {
//                Debug.Log("script.csv�t�@�C����������!!!");
//                //�@Asset��������ǂݍ��ށiResources�ł͂Ȃ��̂Œ��Ӂj
//                TextAsset textasset = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
//                //�@������ScriptableObject�t�@�C����ǂݍ��ށB�Ȃ��ꍇ�͐V���ɍ��B
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
