using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DisplayerScript : MonoBehaviour
{
//LOCAL VARIABLES #############################################################################
    private object currentFood;  
    public string displayerId; 
    public int amount;

    void Start()
    {
        displayerId = "DI" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Debug.Log("Display ID: " + displayerId);
    }

   
    void Update()
    {
        
    }

//FUNCTIONS #############################################################################
}
