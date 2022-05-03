using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelCommand : Command
{
    public bool requiresCancelation = true;
    CancelableCommand command;

    public CancelCommand(CancelableCommand command) : base()
    {
        this.command = command;
    }

    public override Status Execute()
    {
        if(command == null)
        {
            return Status.Failed;
        }

        return command.Cancel();
    }

    public override Status Undo()
    {
        if(command == null)
        {
            return Status.Failed;
        }

        return command.Execute();
    }
}
