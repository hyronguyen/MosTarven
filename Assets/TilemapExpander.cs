using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapExpander : MonoBehaviour
{
    public Tilemap tilemapGrass;
    public Tilemap baseTilemap;
    public Tilemap tilemapWall;         // Thêm tilemap dành cho tường
    public TileBase expandTile;         // Tile nền mở rộng
    public TileBase wallTile;           // Tile tường
    public int expandUnit = 2;          // Số ô mở rộng

    public void ExpandLeft()
    {
        BoundsInt bounds = baseTilemap.cellBounds;
        int leftX = bounds.xMin;

        for (int x = 1; x <= expandUnit; x++)
        {
            int targetX = leftX - x;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(targetX, y, 0);

                // Xóa tile cỏ
                tilemapGrass.SetTile(pos, null);

                // Đặt tile nền base
                baseTilemap.SetTile(pos, expandTile);
            }

            // ✅ Đặt tường tại chính cột mới mở rộng, y = 0
            Vector3Int wallPos = new Vector3Int(targetX+1, 0, 0);
            tilemapWall.SetTile(wallPos, wallTile);
        }
    }

    public void ExpandRight()
    {
        BoundsInt bounds = baseTilemap.cellBounds;
        int bottomY = bounds.yMin;

        for (int y = 1; y <= expandUnit; y++)
        {
            int targetY = bottomY - y;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, targetY, 0);

                tilemapGrass.SetTile(pos, null);
                baseTilemap.SetTile(pos, expandTile);
            }

            // ✅ Đặt tường tại mỗi hàng vừa mở rộng, tại cột x = 1 (hoặc vị trí bạn muốn)
            Vector3Int wallPos = new Vector3Int(1, targetY+1, 0); // x = 1 cố định
            tilemapWall.SetTile(wallPos, wallTile);
            Debug.Log($"[ExpandRight] Đặt tường tại: {wallPos}");
        }
    }


}
