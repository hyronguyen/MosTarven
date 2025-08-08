using UnityEngine;

public class CookerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private int foodId;  // ID c?a m�n ?n ?ang ???c n?u

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Kh�ng t�m th?y Animator tr�n object: " + gameObject.name);
        }

        // C� th? kh?i t?o foodId ? ?�y n?u c?n
        // foodId = -1; // v� d?: -1 l� ch?a n?u g�
    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetBool("isCooking", true);
        }

        Debug.Log("Cooker ?ang n?u m�n c� ID: " + foodId);
    }

    // Cho ph�p script kh�c g�n ID m�n ?n
    public void SetFood(int id)
    {
        foodId = id;
    }

    // L?y ID m�n ?ang n?u
    public int GetFoodId()
    {
        return foodId;
    }
}
