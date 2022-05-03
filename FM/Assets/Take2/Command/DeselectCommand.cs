using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeselectCommand : Command
{
    private FMInfo info;

    public DeselectCommand(FMInfo info) : base()
    {
        this.info = info;
    }

    public override Status Execute()
    {
        SelectionTool.selectionTool.Deselect(info);
        return Status.Success;
    }

    public override Status Undo()
    {
        SelectionTool.selectionTool.Select(info);
        return Status.Success;
    }
}
