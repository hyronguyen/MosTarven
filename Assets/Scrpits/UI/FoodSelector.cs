using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSelector : MonoBehaviour
{
    public Transform contentParent;

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

    void LoadButtons()
    {
       // Load tất cả prefab button trong Resources/FoodSelectList_0
        GameObject[] buttonPrefabs = Resources.LoadAll<GameObject>("FoodSelectList_0");

        Debug.Log("Số prefab load được: " + buttonPrefabs.Length);

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
