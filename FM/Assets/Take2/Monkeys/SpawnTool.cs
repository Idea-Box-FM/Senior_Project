using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnTool : Monkey
{
    public static SpawnTool spawnTool;



    private void Awake()
    {
        SingletonPattern<SpawnTool>(ref spawnTool, this);
        InputManager.inputManager.cameraControl.Editor.Spawn.canceled += Invoke;
    }

    public void Await()
    {
        //activate input
        InputManager.inputManager.SelectEnabled = false;
    }

    public void EndAwait()
    {
        //deactivate input
        InputManager.inputManager.SelectEnabled = true;
    }

    private void Invoke(InputAction.CallbackContext obj)
    {
        //TODO research whether deactivating the button deactivates this aswell
        Preview[] previews = levelEditingManager.group.GetComponentsInChildren<Preview>();

        Command command = null;

        if (previews.Length == 1)
        {
            Preview preview = previews[0];
            command = MakeCommand(preview);
        }
        else if (previews.Length > 1)
        {
            //TODO finish this for multiple preivews
            //foreach(Preview)
        }

        levelEditingManager.Execute(command);
    }

    public override Command MakeCommand(params object[] parameters)
    {
        Preview preview = parameters[0] as Preview;

        return MakeCommand(preview);
    }

    public Command MakeCommand(Preview preview)
    {
        return new SpawnCommand(preview);
    }
    
    public static FMInfo Spawn(FMPrefab prefab, XML xml)
    {
        GameObject gameObject = prefab.InstanciatePrefab(xml);
        gameObject.SetActive(true);
        
        return gameObject.GetComponent<FMInfo>();
    }

    public static void Destroy(FMInfo info)
    {
        Destroy(info.gameObject);
    }
}
