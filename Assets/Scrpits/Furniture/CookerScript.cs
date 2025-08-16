using UnityEngine;
using System;

public class CookerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private int foodId;  
    public string cookerId; 
    private GameObject FoodSelector;

    void Start()
    {
        cookerId = "CO" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Debug.Log("Cooker ID: " + cookerId);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Kh�ng t�m th?y Animator tr�n object: " + gameObject.name);
        }

        Transform parent = GameObject.Find("Canvas").transform;
        FoodSelector = parent.Find("FoodSellect_panel")?.gameObject;
        if (FoodSelector != null)
        {
            Debug.Log("Đã tìm thấy FoodSellect_panel");
        }
        else
        {
            Debug.LogError("Không tìm thấy FoodSellect_panel trong scene");
        }

    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            FoodSelector.SetActive(true);
            Debug.Log("FoodSelector đã được kích hoạt cho Cooker: " + cookerId);
            //animator.SetBool("isCooking", true);
        }
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
