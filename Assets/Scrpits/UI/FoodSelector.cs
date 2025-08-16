using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelector : MonoBehaviour
{
    public Transform contentParent;
    public string cookerId;
    public GameObject foodSelectedPrefab;
    public string foodCode;

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

    public void SetCookerId(string id)
    {
        cookerId = id;
        Debug.Log("Food selectot set cooker IDto: " + cookerId);
    }

    public void SetFoodCode(string foodCode)
    {
        this.foodCode = foodCode;
        Debug.Log("Food selectot set food code to: " + foodCode);
        SetFoodSelectedPrefab(foodCode);

    }
    
    void SetFoodSelectedPrefab(string foodCode)
    {
       this.foodCode = foodCode;
        foodSelectedPrefab = Resources.Load<GameObject>("Foods/" + foodCode);
        if (foodSelectedPrefab == null)
        {
            Debug.LogError("Không tìm thấy prefab món ăn với mã: " + foodCode);
            return;
        }
        Debug.Log("Đã chọn món ăn: " + foodSelectedPrefab.name);
    }

    public void SetFoodForCooker()
    {
        // Tìm tất cả các CookerScript trong scene
        CookerScript[] cookers = FindObjectsOfType<CookerScript>();
        foreach (CookerScript cooker in cookers)
        {
            if (cooker.cookerId == cookerId)
            {
                cooker.SetFood(this.foodSelectedPrefab);
                Debug.Log("Đã gán món ăn ID " + foodSelectedPrefab.name + " cho Cooker ID: " + cooker.cookerId);
                break;
            }
        }
    }

    void LoadButtons()
    {
       // Load tất cả prefab button trong Resources/FoodSelectList_0
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
