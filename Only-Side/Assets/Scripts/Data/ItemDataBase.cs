using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : ScriptableObject
{
    public ItemData[] itemDatas;
}

[System.Serializable]
public class ItemData
{
    public string name;
    public string description;
    public Sprite sprite;
    public float weight;
}