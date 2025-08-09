using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    [Header("Mob's Details")]
    public int level;
    public int health;
    public int maxHealth;
    public int energy;
    public int maxEnergy;
    public string nickname;
    public string type;
    public string description;
    public bool isHired = true;

    [Header("Movement")]
    public Tilemap tilemap;                    // Tilemap nền được truyền từ Inspector
    public float moveSpeed = 0.7f;               // Tốc độ di chuyển của monster

    private HashSet<Vector3Int> validCells;    // Tập hợp các ô hợp lệ để di chuyển
    private Vector3Int currentCell;             // Ô hiện tại của monster trên tilemap
    private Vector3Int targetCell;              // Ô đích monster sẽ di chuyển đến
    private bool isMoving = false;


    private float waitTime = 0f;       // Thời gian cần đợi ở ô đích
    private float waitTimer = 0f;      // Bộ đếm thời gian đợi
    private bool isWaiting = false;    // Trạng thái monster đang đợi
    private Animator monsterAnimator;   // Trạng thái monster đang đợi
    private SpriteRenderer monsterSpriteRenderer;   // Trạng thái monster đang đợi

    // Cửa sổ chi tiết của mob
    public GameObject mobDetailPanel;  // Gán từ Inspector

    // Đường đi
    private List<Vector3Int> pathCells = new List<Vector3Int>();
    private int pathIndex = 0;

    void Start()
    {
        monsterAnimator= GetComponent<Animator>();
        monsterSpriteRenderer =GetComponent<SpriteRenderer>();


        InitializeValidCells();

        // Lấy ô hiện tại của monster trên tilemap (làm tròn vị trí hiện tại)
        Vector3 worldPos = transform.position;
        currentCell = tilemap.WorldToCell(worldPos);

        if (!validCells.Contains(currentCell))
        {
            Debug.LogWarning("Monster đang đứng tại vị trí không hợp lệ trên tilemap.");
            // Đặt lại vị trí monster vào ô hợp lệ gần nhất (ví dụ ô đầu tiên trong tập hợp)
            currentCell = new List<Vector3Int>(validCells)[0];
            transform.position = tilemap.GetCellCenterWorld(currentCell);
        }

        targetCell = currentCell;
    }

    void Update()
    {
        InitializeValidCells();
        if (!isMoving)
        {
            ChooseNextTargetCell();
        }
        else
        {
            MoveToTargetCell();
        }
    }

    void OnMouseDown()
    {
        ViewDetailPanel();
    }

    private void ViewDetailPanel()
    {
      
        if (mobDetailPanel != null)
        {
            mobDetailPanel.SetActive(true);

            Transform nameTextTransform = mobDetailPanel.transform.Find("Name");
            if (nameTextTransform != null)
            {
                Text nameText = nameTextTransform.GetComponent<Text>();
                if (nameText != null)
                {
         
                    nameText.text = this.nickname;
                }
                else
                {
                    Debug.LogWarning("Component Text không tìm thấy trên 'Name'");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy con 'Name' trong mobDetailPanel");
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng có tag 'mobDetailPanel'");
        }
    }

    // Khai thac vùng di chuyển
    private void InitializeValidCells()
    {
        validCells = new HashSet<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin + 1; x < bounds.xMax; x++)  // Bỏ cột ngoài cùng bên trái
        {
            for (int y = bounds.yMin + 1; y < bounds.yMax; y++) // Bỏ hàng cuối cùng
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

    // Chọn ô kế tiếp để đi đến (ví dụ chọn ngẫu nhiên trong các ô lân cận hợp lệ)
    private void ChooseNextTargetCell()
    {
        List<Vector3Int> distantCells = new List<Vector3Int>();

        foreach (var cell in validCells)
        {
            int distance = Mathf.Abs(cell.x - currentCell.x) + Mathf.Abs(cell.y - currentCell.y);
            if (distance >= 3)
            {
                distantCells.Add(cell);
            }
        }

        if (distantCells.Count == 0)
        {
            Debug.LogWarning("Không tìm được ô cách >= 3 ô.");
            return;
        }

        targetCell = distantCells[Random.Range(0, distantCells.Count)];

        // Tính đường đi sử dụng A*
        pathCells = FindPath(currentCell, targetCell);
        if (pathCells == null || pathCells.Count == 0)
        {
            Debug.LogWarning("Không tìm thấy đường đi tới ô đích.");
            return;
        }

        pathIndex = 0;
        isMoving = true;

    }



    // Lấy danh sách các ô láng giềng hợp lệ (trái, phải, trên, dưới)
    private List<Vector3Int> GetValidNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] possibleMoves = new Vector3Int[]
        {
            new Vector3Int(cell.x + 1, cell.y, 0),
            new Vector3Int(cell.x - 1, cell.y, 0),
            new Vector3Int(cell.x, cell.y + 1, 0),
            new Vector3Int(cell.x, cell.y - 1, 0)
        };

        foreach (var nextCell in possibleMoves)
        {
            if (validCells.Contains(nextCell))
            {
                neighbors.Add(nextCell);
            }
        }

        return neighbors;
    }

    //FUNCTION Di chuyển monster đến ô đích và đợi vài giây
    private void MoveToTargetCell()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                monsterAnimator.SetBool("isRunning", false);
                waitTimer = 0f;
                ChooseNextTargetCell();
            }
            return;
        }

        if (pathCells == null || pathIndex >= pathCells.Count)
        {
            // Đường đi hết, bắt đầu đợi
            currentCell = targetCell;
            isMoving = false;
            waitTime = Random.Range(5f, 10f);
            isWaiting = true;
            monsterAnimator.SetBool("isRunning", false);
            waitTimer = 0f;
            return;
        }

        Vector3 targetPos = tilemap.GetCellCenterWorld(pathCells[pathIndex]);
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        monsterAnimator.SetBool("isRunning", true);

        if (targetPos.x > transform.position.x)
        {
            monsterSpriteRenderer.flipX = false;  // Hướng về phải (mặc định)
        }
        else if (targetPos.x < transform.position.x)
        {
            monsterSpriteRenderer.flipX = true;   // Lật ngang sprite để hướng về trái
        }


        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            currentCell = pathCells[pathIndex];
            pathIndex++;
        }
    }

    // FUNCTION Tìm đường
    private List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var openSet = new HashSet<Vector3Int> { start };
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, int>();
        gScore[start] = 0;

        var fScore = new Dictionary<Vector3Int, int>();
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Vector3Int current = GetLowestFScore(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);

            foreach (var neighbor in GetValidNeighbors(current))
            {
                int tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // Không tìm được đường đi
    }

    // ETC FUNCTION ################################################################################################################################################
    private int Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private Vector3Int GetLowestFScore(HashSet<Vector3Int> openSet, Dictionary<Vector3Int, int> fScore)
    {
        Vector3Int lowest = default;
        int minScore = int.MaxValue;

        foreach (var node in openSet)
        {
            int score = fScore.ContainsKey(node) ? fScore[node] : int.MaxValue;
            if (score < minScore)
            {
                minScore = score;
                lowest = node;
            }
        }
        return lowest;
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}
