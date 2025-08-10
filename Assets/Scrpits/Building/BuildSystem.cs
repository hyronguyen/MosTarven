using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    // LOCAL VARIABLES ##########################################################################
    private Tilemap tilemap;                  // Tilemap nền
    public GameObject unitPrefab;          // Prefab cần đặt
    public GameObject ghostUnitPrefab;     // Prefab ghost (trong suốt)

    private GameObject currentGhost;         // Phiên bản ghost đang hoạt động
    private bool isPlacing = false;
    private HashSet<Vector3Int> validCells;
    private bool isFlipped = false; // Thêm biến lưu trạng thái flip

    // START ####################################################################################
    void Start()
    {
         if (tilemap == null)
    {
        GameObject go = GameObject.FindGameObjectWithTag("baseTilemap");
        if (go != null)
        {
            tilemap = go.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Không tìm thấy GameObject với tag 'baseTilemap'!");
        }
    }
        ReCheckValidCell();
    }

    // UPDATE ####################################################################################
    void Update(){
        if (!isPlacing || currentGhost == null) return;

        // Xử lý nhấn R để lật ghost
        if (Input.GetKeyDown(KeyCode.R))
        {
            isFlipped = !isFlipped;
            SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>();
            if (ghostSR != null)
            {
                ghostSR.flipX = isFlipped;
            }
        }

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
        if (!validCells.Contains(cellPos) || BuildManager.Instance.placedObjects.ContainsKey(cellPos))
        {
            currentGhost.SetActive(false);
            return;
        }

        // Click trái để đặt
        if (Input.GetMouseButtonDown(0))
        {
            PlaceUnit(cellPos);
        }
       
        // Click phải để hủy
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacing();
        }
    }

    // FUNCTION ####################################################################################
    // FUNTION - Kiểm tra lại các ô hợp lệ để đặt
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

    }

    // FUNCTION - Bắt đầu đặt đơn vị
    public void StartPlacing()
    {
        if (isPlacing) CancelPlacing();

        isPlacing = true;
        ReCheckValidCell();
        currentGhost = Instantiate(ghostUnitPrefab);
        currentGhost.SetActive(true);

        // Đặt trạng thái flip cho ghost khi bắt đầu
        SpriteRenderer ghostSR = currentGhost.GetComponent<SpriteRenderer>();
        if (ghostSR != null)
        {
            ghostSR.flipX = isFlipped;
        }
    }

    // FUNCTION - Đặt đơn vị tại ô đã chọn
        private void PlaceUnit(Vector3Int cellPos)
    {
        // Không cho đặt nếu không có tile hoặc đã có object ở ô đó
        if (!tilemap.HasTile(cellPos) || BuildManager.Instance.placedObjects.ContainsKey(cellPos)) return;

        // Lấy vị trí thế giới của ô
        Vector3 placePosition = tilemap.GetCellCenterWorld(cellPos);

        Debug.Log($"Đặt đơn vị tại ô: {cellPos} - Vị trí: {placePosition}");

        placePosition.z = 0f;
        GameObject obj = Instantiate(unitPrefab, placePosition, Quaternion.identity);

        int sortingOrder = Mathf.RoundToInt(-(placePosition.y * 1000f) - placePosition.x);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = sortingOrder;
            sr.flipX = isFlipped; // Áp dụng flip cho object thật
        }

        // Ghi lại object đã đặt ở vị trí này
        BuildManager.Instance.placedObjects[cellPos] = obj;

        CancelPlacing();
    }

    // FUNCTION - Hủy đặt đơn vị
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


