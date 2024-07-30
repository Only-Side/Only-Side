using System.Text;
using UnityEditor;
using UnityEngine;

public static class Hierarchy
{
    // オブジェクトのヒエラルキーパスを取得するメソッド
    public static string GetHierarchyPath(Transform obj)
    {
        // ルートオブジェクトに達するまで再帰的に親を辿る
        if (obj.parent == null)
        {
            return "/" + obj.name;
        }
        return GetHierarchyPath(obj.parent) + "/" + obj.name;
    }
}