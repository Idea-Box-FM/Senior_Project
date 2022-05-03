using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Monkey : MonoBehaviour
{
    public static LEM levelEditingManager
    {
        get
        {
            return LEM.levelEditingManager;
        }
    }

    protected static void SingletonPattern<MonkeyType>(ref MonkeyType instance, MonkeyType me) where MonkeyType : Monkey
    {
        if(instance == null)
        {
            instance = me as MonkeyType;
        } else
        {
            Destroy(me);
        }
    }

    /// <summary>
    /// we use params object[] because dynamic parameter was giving an error where there might have been an error with my C# download
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public abstract Command MakeCommand(params object[] parameters);

    protected Command MakeMultiStepCommands(List<object[]> parameters)
    {
        MultiStepCommand multiStepCommand;
        List<Command> commands = new List<Command>();
        
        foreach(object[] parameterSet in parameters)
        {
            Command command = MakeCommand(parameterSet);
            commands.Add(command);
        }

        multiStepCommand = new MultiStepCommand(commands.ToArray());

        return multiStepCommand;
    }
}