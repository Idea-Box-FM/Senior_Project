//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

///* Programmed By Patrick Naatz
// * Note this tool is made generic enough that it only needs one instance
// */
//public class SpawnTool : MonoBehaviour, Monkey
//{
//    public static SpawnTool spawnTool;

//    private void Awake()
//    {
//        if(spawnTool == null)
//        {
//            spawnTool = this;
//        } else
//        {
//            Destroy(this);
//        }
//    }

//    public Command MakeCommand(bool instantInvoke = true)
//    {
//        Command previewCommand = LevelEditingManagerRedo.levelEditingManager.LastCommand;
//        Preview preview = previewCommand.GetParamater<Preview>(0);

//        return new Command(Spawn, Delete, true, instantInvoke: instantInvoke, preview);
//    }

//    #region Spawn
//    private ReturnStatus Spawn(ref object[] parameters)
//    {
//        Preview preview = parameters[0] as Preview;
//        FMInfo prefab = null;

//        ReturnStatus returnStatus = Spawn(ref prefab, preview);

//        parameters[0] = prefab;
//        return returnStatus;
//    }

//    public ReturnStatus Spawn(ref FMInfo prefab, Preview preview)
//    {
//        if (!preview.CanPlace)
//        {
//            return ReturnStatus.Failed;
//        }

//        prefab = preview.Place();
//        Destroy(preview.transform); //We do this because this is how we currently have it set up. If we want to change it so you place multiple objects till canceled then we can.

//        return ReturnStatus.Success;
//    }
//    #endregion

//    #region Delete
//    private ReturnStatus Delete(ref object[] parameters)
//    {
//        FMInfo prefab = parameters[0] as FMInfo;
//        Preview preview = null;

//        ReturnStatus returnStatus = Delete(prefab, ref preview);

//        parameters[0] = preview;

//        return returnStatus;
//    }

//    public ReturnStatus Delete(FMInfo prefab, ref Preview preview)
//    {
//        if (prefab == null)
//        {
//            Debug.LogError("Prefab already deleted");
//            return ReturnStatus.Failed;
//        }

//        Destroy(prefab.gameObject);

//        return ReturnStatus.Success;
//    }
//    #endregion    
//}