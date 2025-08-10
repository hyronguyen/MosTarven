using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelectButton : MonoBehaviour
{
    public Sprite iconIamge;
    public int foodPrice;
    public string foodName;

    public void DebugFood()
    {
        Debug.Log("Food Name: " + foodName);
        Debug.Log("Food Price: " + foodPrice);
        Debug.Log("Food Icon: " + iconIamge.name);
    
        // Đổi ảnh
        GameObject foodImageObj = GameObject.FindGameObjectWithTag("foodImageFoodSelector");
        if (foodImageObj != null)
        {
            Image img = foodImageObj.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = iconIamge;
                Debug.Log("Đã đổi ảnh của foodImageFoodSelector");
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
                    Debug.Log("Đã đổi tên món ăn (UI Text)");
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
                    Debug.Log("Đã đổi giá món ăn (UI Text)");
                }
            
        }
    }
    
}
