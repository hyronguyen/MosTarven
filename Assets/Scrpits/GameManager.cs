using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //LOCAL VARIABLES ###########################################
    public static GameManager Instance { get; private set; }

    public string fileName = "saveData.json";
    private string filePath;

    public string playerName;
    public int coin;
    public int levelReached;

    private Object coinText;

    //LIFE CYCLE ###########################################
    void Awake()
    {   
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        LoadGame();
    }

    //FUNCTION ###########################################
     
    // FUNTION - Lay cap do hien tai cua nguoi choi
    public int GetLevel(){
        return levelReached;
    }

    // FUNTION - Load save data from JSON file
    void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"[GameManager] - LoadGame: {data.playerName}, Coin Score: {data.coin}, Level: {data.levelReached}");

            playerName = data.playerName;
            coin = data.coin;
            levelReached = data.levelReached;

            GameObject coinTextObj = GameObject.FindGameObjectWithTag("coinTextTag");
            Text coinText = coinTextObj.GetComponent<Text>();
            coinText.text = coin.ToString(); // Gán coin vào Text
        }
        else
        {
            Debug.LogWarning($"[GameManager] File save not found at: {filePath}");
        }
    }

// CLASS ##########################################################################
    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int coin;
        public int levelReached;
    }
}
