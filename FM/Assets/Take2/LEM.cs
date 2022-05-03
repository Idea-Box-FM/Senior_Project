using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEM : MonoBehaviour
{
    public static LEM levelEditingManager;

    public Transform group;

    public Command LastCommand
    {
        get
        {
            if(commands.Count == 0)
            {
                return null;
            }

            return commands.Peek();
        }
    }

    Stack<Command> commands = new Stack<Command>();
    Stack<Command> undonCommands = new Stack<Command>();

    public delegate void ButtonNotificationFunction(Command command, Status status);

    List<ButtonNotificationFunction> buttonNotifications = new List<ButtonNotificationFunction>();

    private void SingletonPattern()
    {
        if(levelEditingManager == null)
        {
            levelEditingManager = this;
        } else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SingletonPattern();
    }

    void NotifyButtons(Command command, Status status) //TODO consider adding notification type, Ex. Execute,redo,undo
    {
        foreach(ButtonNotificationFunction notifyButton in buttonNotifications)
        {
            notifyButton.Invoke(command, status);
        }
    }

    public void Execute(Command command)
    {
        Status status = command.Execute();

        switch (status)
        {
            case Status.Success:
                if(LastCommand is CancelableCommand)
                {
                    //Multicommand to cancel
                }
                if(command is CancelableCommand)
                {
                    CancelableCommand.lastCancelableCommand = command as CancelableCommand;
                }
                commands.Push(command);
                undonCommands.Clear();
                break;
            case Status.Failed:
                break;
        }

        NotifyButtons(command, status);
    }

    public void Undo()
    {
        if(LastCommand == null)
        {
            return;
        }

        Command command = LastCommand;
        Status status = command.Undo();

        switch (status)
        {
            case Status.Success:
                commands.Pop();
                undonCommands.Push(command);
                break;
            case Status.Failed:
                break;
        }

        NotifyButtons(command, status);
    }

    public void Redo()
    {
        if(undonCommands.Count == 0)
        {
            return;
        }

        Command command = undonCommands.Pop();
        Status status = command.Execute();

        switch (status)
        {
            case Status.Success:
                commands.Push(command);
                break;
            case Status.Failed:
                undonCommands.Push(command);
                break;
        }

        NotifyButtons(command, status);
    }

    public void Subscribe(ButtonNotificationFunction newButtonNotification)
    {
        Debug.Log("adding button notification function");
        buttonNotifications.Add(newButtonNotification);
    }

    public void AddToGroup(Transform preview, Vector3 pos)
    {
        preview.position = pos + group.position;
        preview.parent = group;
    }

    private void OnDestroy()
    {
        Debug.Log("command count " + commands.Count);
        while(commands.Count > 0)
        {
            Debug.Log(commands.Pop().GetType());
        }
    }
}
