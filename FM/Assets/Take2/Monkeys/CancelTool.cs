using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CancelTool : MonkeyWithButton
{
    public static CancelTool cancelTool;

    private void Awake()
    {
        //InputManager.inputManager.cameraControl.Editor.Cancel.canceled += Invoke;
        SingletonPattern<CancelTool>(ref cancelTool, this);
    }

    //private void Invoke(InputAction.CallbackContext obj)
    //{
    //    Invoke();
    //}

    #region MakeCommand
    public override Command MakeCommand(params object[] parameters)
    {
        CancelableCommand command = parameters[0] as CancelableCommand;

        return MakeCommand(command);
    }

    public Command MakeCommand(CancelableCommand command)
    {
        return new CancelCommand(command);
    }
    #endregion

    public override void Invoke()
    {
        Command command = CancelableCommand.lastCancelableCommand;

        if (command != null)
        {
            command = MakeCommand(command);

            levelEditingManager.Execute(command);
        }
    }

    public override bool ActivationCheck(Command command, Status status)
    {
        //this will not work properly
        return command is CancelableCommand && status == Status.Success;
    }
}
