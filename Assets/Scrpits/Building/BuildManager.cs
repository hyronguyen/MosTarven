using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    // Các cell đã được đặt
    public Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();
    
    public int countLeftExpand = 0;
    public int countRightExpand = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
