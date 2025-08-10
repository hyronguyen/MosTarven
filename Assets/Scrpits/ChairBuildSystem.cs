using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChairBuildSystem : MonoBehaviour
{
    private Tilemap tilemap;
    public GameObject chairPrefab;
    public GameObject ghostChairPrefab;

    private GameObject currentGhost;
    private bool isPlacing = false;
    private HashSet<Vector3Int> validCells;
    private bool isFlipped = false;

    void Start()
    {
        // Tìm tilemap theo tag
        GameObject go = GameObject.FindGameObjectWithTag("baseTilemap");
        if (go != null)
        {
            tilemap = go.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Không tìm thấy GameObject với tag 'baseTilemap'!");
        }
        ReCheckValidCell();
    }

    private void ReCheckValidCell()
    {
        validCells = new HashSet<Vector3Int>();

        // Tìm tất cả bàn
        TableScript[] tables = FindObjectsOfType<TableScript>();
        foreach (var table in tables)
        {
            Vector3Int tableCell = tilemap.WorldToCell(table.transform.position);

            // 2 ô hợp lệ: bên trái và bên trên bàn
            Vector3Int leftCell = tableCell - Vector3Int.left; // Sửa lại ở đây
            Vector3Int upCell = tableCell + Vector3Int.up;

            if (tilemap.HasTile(leftCell))
            {
                validCells.Add(leftCell);
            }
            if (tilemap.HasTile(upCell))
            {
                validCells.Add(upCell);
            }
        }
        Debug.Log($"Tổng số ô hợp lệ để đặt ghế: {validCells.Count}");
    }

    void Update()
    {
        if (!isPlacing || currentGhost == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacing();
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        // Tìm xem cellPos là leftCell hay upCell của bàn nào
        bool isLeftCell = false;
        TableScript[] tables = FindObjectsOfType<TableScript>();
        foreach (var table in tables)
        {
            Vector3Int tableCell = tilemap.WorldToCell(table.transform.position);
            Vector3Int leftCell = tableCell - Vector3Int.left; // Sửa lại ở đây
            Vector3Int upCell = tableCell + Vector3Int.up;
            if (cellPos == leftCell)
            {
                isLeftCell = true;
                break;
            }
        }

        // Tự động flipX cho ghost nếu là leftCell
        SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>();
        if (ghostSR != null)
        {
            ghostSR.flipX = isLeftCell;
        }

        if (!validCells.Contains(cellPos) || BuildManager.Instance.placedObjects.ContainsKey(cellPos))
        {
            currentGhost.SetActive(false);
            return;
        }

        currentGhost.SetActive(true);
        currentGhost.transform.position = tilemap.GetCellCenterWorld(cellPos);

        if (Input.GetMouseButtonDown(0))
        {
            PlaceChair(cellPos, isLeftCell);
        }
    }

    public void StartPlacing()
    {
        if (isPlacing) CancelPlacing();

        isPlacing = true;
        ReCheckValidCell();
        currentGhost = Instantiate(ghostChairPrefab);
        currentGhost.SetActive(true);

        // Đặt trạng thái flip cho ghost khi bắt đầu
        SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>();
        if (ghostSR != null)
        {
            ghostSR.flipX = isFlipped;
        }
    }

    private void PlaceChair(Vector3Int cellPos, bool isLeftCell)
    {
        if (!validCells.Contains(cellPos) || BuildManager.Instance.placedObjects.ContainsKey(cellPos)) return;

        Vector3 placePosition = tilemap.GetCellCenterWorld(cellPos);
        placePosition.z = 0f;
        GameObject obj = Instantiate(chairPrefab, placePosition, Quaternion.identity);

        int sortingOrder = -(cellPos.x * 10000) - cellPos.y;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = sortingOrder;
            sr.flipX = isLeftCell; // flipX nếu là leftCell
        }

        BuildManager.Instance.placedObjects[cellPos] = obj;
        CancelPlacing();
    }

    private void CancelPlacing()
    {
        isPlacing = false;
        if (currentGhost != null)
        {
            Destroy(currentGhost);
            currentGhost = null;
        }
    }
}
