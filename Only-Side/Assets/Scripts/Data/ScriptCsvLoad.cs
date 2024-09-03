using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptCsvLoad : MonoBehaviour
{
    public TextAsset csvFile; // UnityエディタからCSVファイルをアタッチします
    public ScriptDataBase scriptDataBase;

    public void LoadData()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' }); // 行ごとに分割

        scriptDataBase.scriptDatas = new ScriptData[data.Length - 1]; // ヘッダー行を除外

        for (int i = 1; i < data.Length; i++) // 最初の行はヘッダーなのでスキップ
        {
            string[] row = data[i].Split(new char[] { ',' }); // 各行をカンマで分割

            ScriptData script = new ScriptData();
            script.script_text = row[0];
            script.name_text = row[1];
            script.role_text = row[2];
            script.sound_effect = row[3];
            script.text_display_interval = row[4];
            script.choice_1 = row[5];
            script.choice_2 = row[6];
            script.trasition_line_1 = row[7];
            script.trasition_line_2 = row[8];
            script.normal_trasition_line = row[9];
            script.item_restrictions = row[10];

            scriptDataBase.scriptDatas[i - 1] = script;
        }
    }

    private void Start()
    {
        LoadData();
    }
}
