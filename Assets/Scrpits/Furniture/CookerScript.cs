using UnityEngine;
using System;

public class CookerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    public GameObject foodPrefab;  
    public string cookerId; 
    private GameObject FoodSelector;
    public float cookingTime ;
    public bool isCooking = false;

    void Update(){
        if(cookingTime > 0){
            cookingTime -= Time.deltaTime;
            isCooking = true;
            animator.SetBool("isCooking", isCooking);
            Debug.Log("Đang nấu món ăn: " + foodPrefab.name + " Thời gian còn lại: " + cookingTime);
            if(cookingTime <= 0){
                Debug.Log("Món ăn đã sẵn sàng: " + foodPrefab.name);
             isCooking = false;
             animator.SetBool("isCooking", isCooking);
            }
        }
    }

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
            FoodSelector.GetComponent<FoodSelector>().SetCookerId(cookerId);
            
        }
    }

    public void SetFood(GameObject foodPrefab,int cookingTime)
    {

        this.foodPrefab = foodPrefab;
        this.cookingTime = cookingTime;
    }


    public void GetFoodId()
    {
        Debug.Log("Food ID: " + foodPrefab.name);
    }
}
