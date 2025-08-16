using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelectButton : MonoBehaviour
{
    public Sprite iconIamge;
    public int foodPrice;
    public string foodName;
    public string foodDescription;
    public int foodTime;
    public int foodLvToUnlock;
    private string foodCode;

    
    void Start()
        {
            Debug.Log("[FoodSelectButton] Bắt đầu khởi tạo nút: " + gameObject.name);
            int currentLevel = GameManager.Instance.GetLevel();
            Debug.Log("[FoodSelectButton] Level hiện tại: " + currentLevel);
            if (currentLevel < foodLvToUnlock)
            {
                Debug.LogWarning("[FoodSelectButton] Nút này bị khóa vì level hiện tại (" + currentLevel + ") nhỏ hơn level mở khóa (" + foodLvToUnlock + ")");
                return;
            }
        }

    void Awake()
    {

        string objectName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log("[FoodSelectButton] Bắt đầu tìm thông từ JSON cho nút: " + objectName);
       
        TextAsset jsonFile = Resources.Load<TextAsset>("FoodList");
        if (jsonFile == null)
        {
            Debug.LogError("[FoodSelectButton] Không tìm thấy file JSON: Resources/FoodList.json");
            return;
        }

        FoodDataList foodList = JsonUtility.FromJson<FoodDataList>(jsonFile.text);

        // Tìm record có foodButton = object.name
        foreach (FoodData food in foodList.foods)
        {
            if (food.foodButton == objectName)
            {
                foodCode = food.foodCode;
                foodName = food.foodName;
                foodDescription = food.foodDescription;
                foodTime = food.foodTime;
                foodPrice = food.foodPrice;
                foodLvToUnlock = food.foodLvToUnlock;

                Debug.Log($"[FoodSelectButton] Đã tìm thấy dữ liệu cho nút {objectName}: " +
                        $"Code={foodCode}, Name={foodName}, Price={foodPrice}");

           
                Sprite[] allSprites = Resources.LoadAll<Sprite>("FoodIcons/FoodIconSheet");
                if (allSprites == null || allSprites.Length == 0)
                {
                    Debug.LogError("[FoodSelectButton] Không tìm thấy sprite sheet: Resources/FoodIcons/FoodIconSheet.png");
                    break;
                }
                bool foundSprite = false;
                foreach (Sprite sp in allSprites)
                {
                    if (sp.name == food.foodIconSprite)
                    {
                        iconIamge = sp;
                        foundSprite = true;
                        Transform buttonImageTransform = transform.Find("ButtonImage");
                        if (buttonImageTransform != null)
                    {
                        UnityEngine.UI.Image img = buttonImageTransform.GetComponent<UnityEngine.UI.Image>();
                        if (img != null)
                        {
                            img.sprite = iconIamge;
                            Debug.Log("[FoodSelectButton] Đã gán sprite cho ButtonImage của " + objectName);
                        }
                        else
                        {
                            Debug.LogWarning("[FoodSelectButton] Không tìm thấy component Image trong ButtonImage!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("[FoodSelectButton] Không tìm thấy child ButtonImage!");
                    }


                        Debug.Log($"[FoodSelectButton] Đã load sprite: {sp.name} cho {objectName}");
                        break;
                    }
                }

                if (!foundSprite)
                {
                    Debug.LogWarning($"[FoodSelectButton] Không tìm thấy sprite '{food.foodIconSprite}' trong sheet cho {gameObject.name}");
                }

                break;
            }
        }
    }


    public void DebugFood()
    {
     
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
                    Debug.Log("[FoodSelectButton] Hiển thị giá món ăn: " + legacyText.text);
                }
            
        }
        // Đổi text Description
        GameObject foodDesObj = GameObject.FindGameObjectWithTag("foodDesFoodSelector");
        if (foodDesObj != null)
        {
                Text legacyText = foodDesObj.GetComponent<Text>();
                if (legacyText != null)
                {
                    legacyText.text = foodDescription;
                    Debug.Log("[FoodSelectButton] Hiển thị mô tả: " + legacyText.text);
                }
            
        }
        // Đổi text Time
        GameObject foodTimeObj = GameObject.FindGameObjectWithTag("foodTimeFoodSelector");
        if (foodTimeObj != null)
        {
                Text legacyText = foodTimeObj.GetComponent<Text>();
                if (legacyText != null)
                {
                    // Gán vào UI Text
                        legacyText.text = FormatSeconds(foodTime);
                    Debug.Log("[FoodSelectButton] Hiển thị thời gian: " + legacyText.text);
                }
            
        }
     
        // Gọi hàm SetFoodSelectedPrefab trong FoodSelector
        FoodSelector foodSelector = FindObjectOfType<FoodSelector>();
        if (foodSelector != null)
        {
            foodSelector.SetFoodCode(foodCode,foodTime);
            Debug.Log("Đã gọi hàm SetFoodCode trong FoodSelector với mã món ăn: " + foodCode);
        }
        else
        {
            Debug.LogError("Không tìm thấy FoodSelector trong scene.");
        }
    
    }

    public static string FormatSeconds(int totalSeconds)
{
    System.TimeSpan ts = System.TimeSpan.FromSeconds(totalSeconds);

    if (ts.Days > 0)
    {
        return string.Format("{0}d {1}h {2}m {3}s", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
    }
    else if (ts.Hours > 0)
    {
        return string.Format("{0}h {1}m {2}s", ts.Hours, ts.Minutes, ts.Seconds);
    }
    else if (ts.Minutes > 0)
    {
        return string.Format("{0}m {1}s", ts.Minutes, ts.Seconds);
    }
    else
    {
        return string.Format("{0}s", ts.Seconds);
    }
}

    

    [System.Serializable]
    public class FoodData
    {
        public string foodButton;
        public string foodCode;
        public string foodName;
        public string foodIconSprite;
        public string foodDescription;
        public int foodTime;
        public int foodPrice;
        public int foodLvToUnlock;
    }

    [System.Serializable]
    public class FoodDataList
    {
        public FoodData[] foods;
    }

}


