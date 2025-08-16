using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelector : MonoBehaviour
{
     // LOCAL VARIBLE #################################################
    public Transform contentParent;
    public string cookerId;
    public GameObject foodSelectedPrefab;
    public string foodCode;
    public int foodTime;

    // LIFE CYCLE #################################################
    void Start()
    {
        LoadButtons();
    }
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

     // FUNCTIONS #################################################
  
    public void SetCookerId(string id)
    {
        cookerId = id;
        Debug.Log("[FoodSelector] Đang mở từ Cooker ID: " + cookerId);
    }

    public void SetFoodCode(string foodCode, int foodTime)
    {
        this.foodTime = foodTime;
        this.foodCode = foodCode;
        Debug.Log("[FoodSelector] Food hiện đang chọn: " + foodCode);
        SetFoodSelectedPrefab(foodCode);
    }
    
    void SetFoodSelectedPrefab(string foodCode)
    {
       this.foodCode = foodCode;
        foodSelectedPrefab = Resources.Load<GameObject>("Foods/" + foodCode);
        if (foodSelectedPrefab == null)
        {
            Debug.LogError("[FoodSelector] Không tìm thấy prefab món ăn với mã: " + foodCode);
            return;
        }
        Debug.Log("[FoodSelector] Đã chọn món ăn: " + foodSelectedPrefab.name);
    }

    public void SetFoodForCooker()
    {
        // Tìm tất cả các CookerScript trong scene
        CookerScript[] cookers = FindObjectsOfType<CookerScript>();
        foreach (CookerScript cooker in cookers)
        {
            if (cooker.cookerId == cookerId)
            {
                cooker.SetFood(this.foodSelectedPrefab,this.foodTime);
                Debug.Log("[FoodSelector] Đã gán món ăn " + foodSelectedPrefab.name + " cho Cooker ID: " + cooker.cookerId);
                 gameObject.SetActive(false);
                break;
            }
        }
    }

    void LoadButtons()
    {
        GameObject[] buttonPrefabs = Resources.LoadAll<GameObject>("FoodSelectList_0");

        foreach (GameObject prefab in buttonPrefabs)
        {
            GameObject btnObj = Instantiate(prefab, contentParent);

            // Lấy component Button
            Button btn = btnObj.GetComponent<Button>();
            // Lấy script FoodSelectButton
            FoodSelectButton foodBtn = btnObj.GetComponent<FoodSelectButton>();

            if (btn != null && foodBtn != null)
            {
                btn.onClick.AddListener(foodBtn.DebugFood);
            }
        }
    }
}
