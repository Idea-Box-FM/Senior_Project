using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTool : MonkeyWithButton
{
    public static PreviewTool previewTool;

    private void Awake()
    {
        SingletonPattern<PreviewTool>(ref previewTool, this);
    }

    #region Invoke
    public void Invoke(FMPrefab fmPrefab, Vector3? pos = null)
    {
        pos ??= Vector3.zero;

        Command command = MakeCommand(fmPrefab);
        levelEditingManager.Execute(command);
    }

    /// <summary>
    /// This function exists only for button usage
    /// </summary>
    /// <param name="fMPrefab"></param>
    public void Invoke(FMPrefab fMPrefab)
    {
        Invoke(fMPrefab, null);
    }
    #endregion

    #region MakeCommand
    public override Command MakeCommand(params object[] parameters)
    {
        FMPrefab prefab = parameters[0] as FMPrefab;

        return MakeCommand(prefab);
    }

    public Command MakeCommand(FMPrefab prefab)
    {
        return new PreviewCommand(prefab);
    }
    #endregion

    #region Command Functions
    public static Preview Preview(FMPrefab prefab, Vector3 relativePosition)
    {

        Preview preview = prefab.InstanciateExample().GetComponent<Preview>();
        levelEditingManager.AddToGroup(preview.transform, relativePosition);

        SpawnTool.spawnTool.Await();

        return preview;
    }

    public static void CancelPreview(Preview preview)
    {
        //remove input from input manager
        SpawnTool.spawnTool.EndAwait();

        Destroy(preview.gameObject);
    }

    /// <summary>
    /// Do not call this function call with FMPrefab
    /// </summary>
    public override void Invoke(){}

    /// <summary>
    /// Leave this function as not implemented. the function that calls it in the Observer Button should be overwritten
    /// </summary>
    /// <param name="command"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public override bool ActivationCheck(Command command, Status status)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}