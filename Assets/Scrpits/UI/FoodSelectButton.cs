using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelectButton : MonoBehaviour
{
    public Sprite iconIamge;
    public int foodPrice;
    public string foodName;
    private string foodCode;

    void Start()
    {
     foodCode = iconIamge.name; 
    }

    public void DebugFood()
    {
        Debug.Log("Food Name: " + foodName);
        Debug.Log("Food Price: " + foodPrice);
        Debug.Log("Food Icon: " + iconIamge.name);
        Debug.Log("Food Code: " + foodCode);
    
        // Đổi ảnh
        GameObject foodImageObj = GameObject.FindGameObjectWithTag("foodImageFoodSelector");
        if (foodImageObj != null)
        {
            Image img = foodImageObj.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = iconIamge;
    
            }
        }

        // Đổi text tên món ăn
        GameObject foodTextObj = GameObject.FindGameObjectWithTag("foodTextFoodSelector");
        if (foodTextObj != null)
        {
            
                Text legacyText = foodTextObj.GetComponent<Text>();
                if (legacyText != null)
                {
                    legacyText.text = foodName;
               
                }
            
        }

        // Đổi text giá
        GameObject foodPriceObj = GameObject.FindGameObjectWithTag("foodPriceFoodSelector");
        if (foodPriceObj != null)
        {
            
                Text legacyText = foodPriceObj.GetComponent<Text>();
                if (legacyText != null)
                {
                    legacyText.text = foodPrice.ToString();
                }
            
        }
     
        // Gọi hàm SetFoodSelectedPrefab trong FoodSelector
        FoodSelector foodSelector = FindObjectOfType<FoodSelector>();
        if (foodSelector != null)
        {
            foodSelector.SetFoodCode(foodCode);
            Debug.Log("Đã gọi hàm SetFoodCode trong FoodSelector với mã món ăn: " + foodCode);
        }
        else
        {
            Debug.LogError("Không tìm thấy FoodSelector trong scene.");
        }
    
    }
    
}
