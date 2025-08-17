using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableScript : MonoBehaviour
{
    public bool hasChair = false;
    public string tableId;
    // Start is called before the first frame update
    void Start()
    {
         tableId = "TA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Debug.Log("[TableScript] Khởi tạo bàn với tableId: " + tableId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHasChair(bool value)
    {
        hasChair = value;
        Debug.Log("[TableScript] Đã cập nhật trạng thái hasChair: " + hasChair);
    }
}
