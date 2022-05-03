//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CancelTool : MonoBehaviour, Monkey
//{
//    public static CancelTool cancelTool;

//    void Awake()
//    {
//        if(cancelTool == null)
//        {
//            cancelTool = this;
//        } else
//        {
//            Destroy(this);
//        }
//    }

//    public Command MakeCommand(bool instantInvoke = true)
//    {
//        return new CancelCommand(instantInvoke);
//    }
//}
