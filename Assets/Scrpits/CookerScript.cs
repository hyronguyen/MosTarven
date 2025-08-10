using UnityEngine;

public class CookerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private int foodId;  // ID c?a món ?n ?ang ???c n?u

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Không tìm th?y Animator trên object: " + gameObject.name);
        }

        // Có th? kh?i t?o foodId ? ?ây n?u c?n
        // foodId = -1; // ví d?: -1 là ch?a n?u gì
    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetBool("isCooking", true);
        }

        Debug.Log("Cooker ?ang n?u món có ID: " + foodId);
    }

    // Cho phép script khác gán ID món ?n
    public void SetFood(int id)
    {
        foodId = id;
    }

    // L?y ID món ?ang n?u
    public int GetFoodId()
    {
        return foodId;
    }
}
