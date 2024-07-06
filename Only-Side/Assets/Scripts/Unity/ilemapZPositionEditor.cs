using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TilemapZPositionEditor : EditorWindow
{
    [MenuItem("Window/Tilemap Z Position Editor")]
    public static void ShowWindow()
    {
        GetWindow<TilemapZPositionEditor>("Tilemap Z Position Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilemap Z Position Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Increase Z Position (Shift+Up Arrow)"))
        {
            IncreaseZPosition();
        }

        if (GUILayout.Button("Decrease Z Position (Shift+Down Arrow)"))
        {
            DecreaseZPosition();
        }
    }

    [MenuItem("Edit/Increase Tilemap Z Position %&UP")]
    private static void IncreaseZPosition()
    {
        AdjustZPosition(1);
    }

    [MenuItem("Edit/Decrease Tilemap Z Position %&DOWN")]
    private static void DecreaseZPosition()
    {
        AdjustZPosition(-1);
    }

    private static void AdjustZPosition(int amount)
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        Tilemap tilemap = selectedObject.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogWarning("Selected GameObject does not have a Tilemap component.");
            return;
        }

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    if (tilemap.HasTile(position))
                    {
                        TileBase tile = tilemap.GetTile(position);
                        Vector3Int newPosition = new Vector3Int(position.x, position.y, position.z + amount);
                        tilemap.SetTile(position, null);
                        tilemap.SetTile(newPosition, tile);
                    }
                }
            }
        }

        Debug.Log($"Tilemap Z position adjusted by {amount}.");
    }
}
