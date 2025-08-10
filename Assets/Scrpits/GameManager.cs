using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string fileName = "saveData.json";
    private string filePath;

    // 👉 Các biến lưu trạng thái game đọc từ file
    public string playerName;
    public int coin;
    public int levelReached;

    private Object coinText;



    void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "saveData.json");
        LoadGame();
    }
     
    void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"[LoadGame] Player: {data.playerName}, Coin Score: {data.coin}, Level: {data.levelReached}");

            playerName = data.playerName;
            coin = data.coin;

            GameObject coinTextObj = GameObject.FindGameObjectWithTag("coinTextTag");
            Text coinText = coinTextObj.GetComponent<Text>();
            coinText.text = coin.ToString(); // Gán coin vào Text
        }
        else
        {
            Debug.LogWarning($"[LoadGame] File not found at: {filePath}");
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int coin;
        public int levelReached;
    }
}
