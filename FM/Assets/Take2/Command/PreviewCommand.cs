using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCommand : CancelableCommand
{

    #region fields
    #region execute fields
    FMPrefab prefab;
    Vector3 relativePosition;
    #endregion
    #region undo fields
    Preview preview;
    #endregion
    #endregion

    public PreviewCommand(FMPrefab prefab, Vector3? relativePosition = null) : base()
    {
        this.prefab = prefab;
        this.relativePosition = relativePosition ?? Vector3.zero;
    }

    public override Status Execute()
    {
        preview = PreviewTool.Preview(prefab, relativePosition);

        return Status.Success;
    }

    public override Status Undo()
    {
        PreviewTool.CancelPreview(preview);
        preview = null;

        return Status.Success;
    }
}
