using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDataBase : ScriptableObject
{
    public ScriptData[] scriptDatas;
}

[System.Serializable]
public class ScriptData
{
    public string script_text;
    public string name_text;
    public string role_text;
    public string sound_effect;
    public string text_display_interval;
    public string choice_1;
    public string choice_2;
    public string trasition_line_1;
    public string trasition_line_2;
    public string normal_trasition_line;
    public string choice_item_restrictions;
}
