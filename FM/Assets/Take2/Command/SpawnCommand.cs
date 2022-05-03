using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCommand : Command
{
    FMPrefab prefab;
    XML xml;

    FMInfo info;

    bool CanPlace
    {
        get
        {
            return prefab != null;
        }
    }

    public SpawnCommand(Preview preview) : base()
    {
        if (preview.CanPlace) //we do it this way to save memory and runtime
        {
            prefab = preview.prefab;
            this.xml = preview.ConvertToXML();
        }
    }

    public override Status Execute()
    {
        if (!CanPlace)
        {
            return Status.Failed;
        }

        info = SpawnTool.Spawn(prefab, xml);
        return Status.Success;
    }

    public override Status Undo()
    {
        SpawnTool.Destroy(info);
        return Status.Success;
    }
}
