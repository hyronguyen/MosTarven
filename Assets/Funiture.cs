using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funiture : MonoBehaviour
{
    public string furnitureCode;
    public string FunitureType;

    public void SetFuniture(string code, string type)
    {
        furnitureCode = code;
        FunitureType = type;
    }

    public string GetFunitureCode(){
        return furnitureCode;
    }

}
