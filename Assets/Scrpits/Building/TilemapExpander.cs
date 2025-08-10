using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapExpander : MonoBehaviour
{
    public Tilemap tilemapGrass;
    public Tilemap baseTilemap;
    public Tilemap tilemapWall;         // Thêm tilemap dành cho tường
    public Tilemap tilemapWall2;        // Thêm tilemap tường thứ 2
    public TileBase expandTile;         // Tile nền mở rộng
    public TileBase wallTile;           // Tile tường
    private int expandUnit = 2;          // Số ô mở rộng

    public void ExpandLeft()
    {
        if (BuildManager.Instance.countLeftExpand >= 10)
        {
            Debug.LogWarning("Đã đạt giới hạn mở rộng bên trái!");
            return;
        }

        BoundsInt bounds = baseTilemap.cellBounds;
        int leftX = bounds.xMin;

        for (int x = 1; x <= expandUnit; x++)
        {
            int targetX = leftX - x;

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(targetX, y, 0);

                tilemapGrass.SetTile(pos, null);
                baseTilemap.SetTile(pos, expandTile);
            }

            Vector3Int wallPos = new Vector3Int(targetX + 1, 0, 0);
            tilemapWall.SetTile(wallPos, wallTile);

            Vector3Int wall2Pos = new Vector3Int(targetX + 1, 0, 0);
            tilemapWall2.SetTile(wall2Pos, wallTile);
        }

        BuildManager.Instance.countLeftExpand++;
    }

    public void ExpandRight()
    {
        if (BuildManager.Instance.countRightExpand >= 10)
        {
            Debug.LogWarning("Đã đạt giới hạn mở rộng bên phải!");
            return;
        }

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

            Vector3Int wallPos = new Vector3Int(1, targetY + 1, 0);
            tilemapWall.SetTile(wallPos, wallTile);

            Vector3Int wall2Pos = new Vector3Int(1, targetY + 1, 0);
            tilemapWall2.SetTile(wall2Pos, wallTile);

            Debug.Log($"[ExpandRight] Đặt tường tại: {wallPos}, wall2 tại: {wall2Pos}");
        }

        BuildManager.Instance.countRightExpand++;
    }


}
