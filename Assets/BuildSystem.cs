using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    public Tilemap tilemap;                  // Tilemap nền
    public GameObject cookerPrefab;          // Prefab cần đặt
    public GameObject ghostCookerPrefab;     // Prefab ghost (trong suốt)

    private GameObject currentGhost;         // Phiên bản ghost đang hoạt động
    private bool isPlacing = false;
    private HashSet<Vector3Int> validCells;
    private Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();


    void Start()
    {
        ReCheckValidCell();
    }

    private void ReCheckValidCell()
    {
        validCells = new HashSet<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin + 1; x < bounds.xMax; x++)  // Bỏ cột ngoài cùng bên trái
        {
            for (int y = bounds.yMin + 1; y < bounds.yMax; y++) // Bỏ hàng cuối cùng y = yMax - 1
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cell))
                {
                    validCells.Add(cell);

                }
            }
        }

        Debug.Log($"Tổng số ô hợp lệ: {validCells.Count}");
    }



    void Update()
    {
        if (!isPlacing || currentGhost == null) return;

        // Lấy vị trí chuột theo tile
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        // Kiểm tra ô hiện tại có nằm trong danh sách hợp lệ không
        if (!validCells.Contains(cellPos))
        {
            currentGhost.SetActive(false);
            return;
        }

        currentGhost.SetActive(true);
        currentGhost.transform.position = tilemap.GetCellCenterWorld(cellPos);
        // Kiểm tra ô hợp lệ và chưa có object
        if (!validCells.Contains(cellPos) || placedObjects.ContainsKey(cellPos))
        {
            currentGhost.SetActive(false);
            return;
        }


        // Click trái để đặt
        if (Input.GetMouseButtonDown(0))
        {
            PlaceCooker(cellPos);
        }
        Debug.Log(cellPos);

        // Click phải để hủy
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacing();
        }

    }

    public void StartPlacing()
    {
        if (isPlacing) CancelPlacing();

        isPlacing = true;
        ReCheckValidCell();
        currentGhost = Instantiate(ghostCookerPrefab);
        currentGhost.SetActive(true);
    }

    private void PlaceCooker(Vector3Int cellPos)
    {
        // Không cho đặt nếu không có tile hoặc đã có object ở ô đó
        if (!tilemap.HasTile(cellPos) || placedObjects.ContainsKey(cellPos)) return;

        Vector3 placePosition = tilemap.GetCellCenterWorld(cellPos);
        placePosition.z = 0f;
        GameObject obj = Instantiate(cookerPrefab, placePosition, Quaternion.identity);

        int sortingOrder = -(cellPos.x * 10000) - cellPos.y;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = sortingOrder;
        }

        // Ghi lại object đã đặt ở vị trí này
        placedObjects[cellPos] = obj;

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


