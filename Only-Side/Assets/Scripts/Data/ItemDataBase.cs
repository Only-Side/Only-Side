using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "ScriptableObject/ItemDataBase")]
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