using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status{
    Success,
    Failed
}

public abstract class Command
{
    public Command(){}

    public abstract Status Execute();
    public abstract Status Undo();
}
