using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeInput_String : GenericType<string>
{
    //[SerializeField]
   // public string data;

    //[SerializeField]
    //public T GetData()
    //{
    //    if (data is string)
    //    {
    //        Debug.Log("Data is string!");
    //    }
    //    else
    //    {
    //        Debug.Log("Data is not string!");
    //    }

    //    return data;
    //}
}

//public class GenericString<T>
//{
//    public string value;
//}

//public class GenericInt<T>
//{
//    public int value;
//}

public class GenericType<T>
{
    public T value;
}

