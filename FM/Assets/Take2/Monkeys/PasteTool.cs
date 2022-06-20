using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteTool : MonkeyWithButton
{
    public static PasteTool pasteTool;

    public override bool ActivationCheck(Command command, Status status)
    {
        throw new System.NotImplementedException();
    }

    public void Awake()
    {
        SingletonPattern<PasteTool>(ref pasteTool, this);
    }

    public override void Invoke()
    {
        throw new System.NotImplementedException();
    }

    public override Command MakeCommand(params object[] parameters)
    {
        throw new System.NotImplementedException();
    }
}
