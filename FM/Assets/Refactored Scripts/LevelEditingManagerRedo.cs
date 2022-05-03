//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class LevelEditingManagerRedo : MonoBehaviour
//{
//    public static LevelEditingManagerRedo levelEditingManager;

//    Stack<Command> commands = new Stack<Command>();
//    Stack<Command> undoneCommands = new Stack<Command>();

//    public Command LastCommand
//    {
//        get
//        {
//            return commands.Peek();
//        }
//    }

//    [SerializeField] Transform group;

//    void Awake()
//    {
//        //singleton pattern
//        if(levelEditingManager == null)
//        {
//            levelEditingManager = this;
//        } else
//        {
//            Destroy(this);
//        }
//    }

//    /// <summary>
//    /// Call this command to invoke commands
//    /// </summary>
//    /// <param name="command">prebuilt command made by the monkey</param>
//    public void Do(Command command)
//    {
//        ReturnStatus status = command.Do();

//        switch (status)
//        {
//            case ReturnStatus.Success:
//                if (undoneCommands.Count > 0)
//                {
//                    undoneCommands.Clear();
//                }

//                if (LastCommand.requiresCancelation)
//                {
//                    command = AddCancelation(command);
//                }
//                commands.Push(command);
//                break;
//            case ReturnStatus.Failed:
//                Debug.Log("This command failed");
//                break;
//        }
//    }

//    private Command AddCancelation(Command command)
//    {//UPDATE NEW MULTISTEP TO USE THE LAST COMMANDS FLAG PROPERTIES
//        MultiStepCommand multiStepCommand;
//        Command cancelCommand = CancelTool.cancelTool.MakeCommand(false);

//        if (command is MultiStepCommand)
//        {
//            multiStepCommand = command as MultiStepCommand;
//            List<Command> commands = multiStepCommand.commands.ToList();
//            commands.Insert(0, cancelCommand);
//            multiStepCommand.commands = commands.ToArray();
//        }
//        else
//        {
//            Command[] commands = {
//                cancelCommand,
//                command 
//            };

//            multiStepCommand = new MultiStepCommand(commands);
//        }

//        return multiStepCommand;
//    }

//    public void Undo()
//    {
//        Command command = commands.Pop();

//        command.Undo();

//        undoneCommands.Push(command);
//    }

//    public void Redo()
//    {
//        Command command = undoneCommands.Pop();

//        Do(command);
//    }

//    public void ReprovokeLastCommand()
//    {
//        Command command = commands.Pop();

//        Do(command);
//    }

//    public Vector3 GetRelativePositionFromCenter(Vector3 position)
//    {
//        return position - group.position;
//    }

//    public Vector3 CalculateNewPosition(Vector3 RelativePosition)
//    {
//        return group.position + RelativePosition;
//    }
//}
