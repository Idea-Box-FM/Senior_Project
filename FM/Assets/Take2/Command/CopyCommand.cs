using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCommand : Command
{

    List<XML> copyInfo = new List<XML>();

    public CopyCommand(List<XML> newCopyInfo) : base()
    {
        copyInfo = newCopyInfo;
    }

    public override Status Execute()
    {
        List<XML> oldCopyInfo = CopyTool.copyTool.Copy(copyInfo);

        if (oldCopyInfo.Count != 0)
        {
            copyInfo = oldCopyInfo;
        }

        return Status.Success;
    }

    public override Status Undo()
    {
        List<XML> newCopyInfo = CopyTool.copyTool.UnCopy(copyInfo);
        copyInfo = newCopyInfo;

        return Status.Success;
    }
}
