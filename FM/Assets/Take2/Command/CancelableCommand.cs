using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CancelableCommand : Command
{
    public static CancelableCommand lastCancelableCommand;

    public CancelableCommand() : base(){
        
    }

    public Status Cancel()
    {
        //LEM.levelEditingManager.Undo();
        //return Status.Success;
        if(this == lastCancelableCommand)
        {
            lastCancelableCommand = null;
        }

        return Undo();
    }
}
