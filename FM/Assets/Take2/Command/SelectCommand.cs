using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCommand : Command
{
    FMInfo info;

    public SelectCommand(FMInfo info) : base()
    {
        this.info = info;
    }

    public override Status Execute()
    {
        SelectionTool.selectionTool.Select(info);
        return Status.Success;
    }

    public override Status Undo()
    {
        SelectionTool.selectionTool.Deselect(info);
        return Status.Success;
    }
}
