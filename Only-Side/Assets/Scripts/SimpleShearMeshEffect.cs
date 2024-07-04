using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シンプルなuGUI要のシア―(Shear)変換(せん断)
/// </summary>
[RequireComponent(typeof(Graphic))]
public class SimpleShearMeshEffect : BaseMeshEffect
{
    [Tooltip("縦横の倍率: Pivotが変換軸、また反映立xはheightにyはwidthに比例する")]
    public Vector2 shear = new Vector2(0.2f, 0.0f);

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        // 頂点数
        var vertCount = vh.currentVertCount;
        // UIの頂点
        UIVertex vert = new UIVertex();
        // すべての頂点に対して
        for (int i = 0; i < vertCount; i++)
        {
            vh.PopulateUIVertex(ref vert, i);
            var pos = vert.position;

            // 頂点のy座標をもとに横にずらす
            pos.x += shear.x * pos.y;
            // 頂点のx座標をもとに縦にずらす
            pos.y += shear.y * pos.x;

            // UIに頂点変更値を保存
            vert.position = pos;
            vh.SetUIVertex(vert, i);
        }
    }
}