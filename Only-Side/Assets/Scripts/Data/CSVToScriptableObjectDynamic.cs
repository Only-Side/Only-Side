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
//        // CSV�t�@�C���̃p�X
//        string filePath = EditorUtility.OpenFilePanel("Select CSV file", "Assets/Resources/CSV", "csv");
//        if (string.IsNullOrEmpty(filePath)) return;

//        // CSV�t�@�C����ǂݍ���
//        string[] csvData = File.ReadAllLines(filePath);
//        if (csvData.Length == 0) return;

//        // �w�b�_�[�s���擾
//        string[] headers = csvData[0].Split(',');

//        // �w�b�_�[�Ɋ�Â���Scriptable Object�̌^�𓮓I�ɒ�`
//        string className = Path.GetFileNameWithoutExtension(filePath);
//        Type dynamicType = CreateDynamicType(className, headers);

//        // �f�[�^�s������
//        for (int i = 1; i < csvData.Length; i++)
//        {
//            string[] dataFields = csvData[i].Split(',');

//            // Scriptable Object�̃C���X�^���X�𐶐�
//            ScriptableObject dataObject = ScriptableObject.CreateInstance(dynamicType);

//            for (int j = 0; j < headers.Length; j++)
//            {
//                string header = headers[j];
//                string value = dataFields[j];

//                // �t�B�[���h�ɒl���Z�b�g
//                FieldInfo field = dynamicType.GetField(header);
//                if (field != null)
//                {
//                    object convertedValue = Convert.ChangeType(value, field.FieldType);
//                    field.SetValue(dataObject, convertedValue);
//                }
//            }

//            // Scriptable Object���A�Z�b�g�Ƃ��ĕۑ�
//            string assetPath = $"Assets/Data/{className}_{i}.asset";
//            AssetDatabase.CreateAsset(dataObject, assetPath);
//        }

//        // �ύX��ۑ����ăG�f�B�^�[�ɔ��f
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }

//    private static Type CreateDynamicType(string className, string[] fields)
//    {
//        // �V�����^�𐶐����邽�߂�TypeBuilder���g�p
//        AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
//        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
//        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
//        TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(ItemData));

//        // �t�B�[���h��ǉ�
//        foreach (string field in fields)
//        {
//            typeBuilder.DefineField(field, typeof(string), FieldAttributes.Public);
//        }

//        return typeBuilder.CreateType();
//    }
//}
