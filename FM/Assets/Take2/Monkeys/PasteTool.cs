using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteTool : MonkeyWithButton
{
    public static PasteTool pasteTool;

    public void Awake()
    {
        SingletonPattern<PasteTool>(ref pasteTool, this);
    }

    public override Command MakeCommand(params object[] parameters)
    {

    }
}
