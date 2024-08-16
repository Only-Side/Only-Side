//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Reflection;
//using System;
//using System.Reflection.Emit;

//public class CSVToScriptableObjectDynamic : MonoBehaviour
//{
//    [MenuItem("Tools/Import CSV to Scriptable Object (Dynamic)")]
//    public static void ImportCSV()
//    {
//        // CSVファイルのパス
//        string filePath = EditorUtility.OpenFilePanel("Select CSV file", "Assets/Resources/CSV", "csv");
//        if (string.IsNullOrEmpty(filePath)) return;

//        // CSVファイルを読み込み
//        string[] csvData = File.ReadAllLines(filePath);
//        if (csvData.Length == 0) return;

//        // ヘッダー行を取得
//        string[] headers = csvData[0].Split(',');

//        // ヘッダーに基づいてScriptable Objectの型を動的に定義
//        string className = Path.GetFileNameWithoutExtension(filePath);
//        Type dynamicType = CreateDynamicType(className, headers);

//        // データ行を処理
//        for (int i = 1; i < csvData.Length; i++)
//        {
//            string[] dataFields = csvData[i].Split(',');

//            // Scriptable Objectのインスタンスを生成
//            ScriptableObject dataObject = ScriptableObject.CreateInstance(dynamicType);

//            for (int j = 0; j < headers.Length; j++)
//            {
//                string header = headers[j];
//                string value = dataFields[j];

//                // フィールドに値をセット
//                FieldInfo field = dynamicType.GetField(header);
//                if (field != null)
//                {
//                    object convertedValue = Convert.ChangeType(value, field.FieldType);
//                    field.SetValue(dataObject, convertedValue);
//                }
//            }

//            // Scriptable Objectをアセットとして保存
//            string assetPath = $"Assets/Data/{className}_{i}.asset";
//            AssetDatabase.CreateAsset(dataObject, assetPath);
//        }

//        // 変更を保存してエディターに反映
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }

//    private static Type CreateDynamicType(string className, string[] fields)
//    {
//        // 新しい型を生成するためにTypeBuilderを使用
//        AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
//        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
//        TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(ItemData));

//        // フィールドを追加
//        foreach (string field in fields)
//        {
//            typeBuilder.DefineField(field, typeof(string), FieldAttributes.Public);
//        }

//        return typeBuilder.CreateType();
//    }
//}
