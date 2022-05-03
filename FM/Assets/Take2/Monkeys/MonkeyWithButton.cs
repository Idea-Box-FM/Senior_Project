using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonkeyWithButton : Monkey
{
    public abstract void Invoke();
    public abstract bool ActivationCheck(Command command, Status status);
}
